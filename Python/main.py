import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
import os

filesPath = "./trazas/"
filesExtension = "json"

def plotBarChart(df, title, ylabel, xlabel, bar_color):
    ax = df.plot(kind="bar", legend=False, color=bar_color, figsize=(8, 6))
    for p in ax.patches:
        ax.annotate(f'{p.get_height()}', (p.get_x() + p.get_width() / 2., p.get_height()), ha='center', va='center', xytext=(0, 10), textcoords='offset points')
    plt.title(title)
    plt.ylabel(ylabel)
    plt.xlabel(xlabel)
    plt.xticks(rotation=0)
    # plt.show()
    plt.savefig(title + '.png')

###############################
##### CARGA DE DATOS
###############################

# Cargar en un dataframe todos los archivos de un formato especifico del directorio
def loadAllFiles(path, extension = "json", sortBy = "eventId"):
  allFilesDf = pd.DataFrame()

  # Recorrer todos los archivos del directorio
  for file_name in os.listdir(path):
    # Si el archivo tiene la extension indicada
    if (file_name.endswith(extension)):
      fileDf = pd.DataFrame()

      # Intenta leer el archivo. Si hay algun error, el dataframe estara vacio
      try:
        if (extension == "json"):
          fileDf = pd.read_json(path + file_name)
      except:
        pass

      # Si el dataframe esta vacio
      if (not fileDf.empty):
        # Se ordenan los eventos (por defecto por eventId)
        fileDf = fileDf.sort_values(by=[sortBy])
        # Se unen los datasets
        allFilesDf = pd.concat([allFilesDf, fileDf], ignore_index=True)

  return allFilesDf


# Devuelve un dataframe con todas las filas entre la primera instancia encontrada en la columna
# parameter del firstValue y la primera instancia encontrada en la columna parameter del lastValue
# (incluyendo las filas con firstValue y lastValue)
def getEventsBetween(dataframe, parameter, firstValue, lastValue):
  # Buscar el primer y ultimo indice de la fila
  start_idx = dataframe[dataframe[parameter] == firstValue].index
  end_idx = dataframe[dataframe[parameter] == lastValue].index

  # Se juntan todas las filas entre ambos eventos
  data = pd.DataFrame()
  for i, j in zip(start_idx, end_idx):
      data = pd.concat([data, dataframe[i:j + 1]], ignore_index=True)

  # display(data)
  return data


df = loadAllFiles(filesPath, filesExtension)
getEventsBetween(df, 'eventName', 'LEVEL_START', 'LEVEL_END')

###############################
##### CALCULO DE METRICAS
###############################

def getNumLevels(dataframe):
  return (dataframe["eventName"] == "LEVEL_START").sum()

def getNumLevelsCompleted(dataframe):
  return (dataframe["eventName"] == "SONG_END").sum()

def getNumLevelsQuit(dataframe):
  return (dataframe["eventName"] == "LEVEL_QUIT").sum()

def getParameterCount(dataframe, parameter, value):
  return dataframe[parameter].value_counts().get(value,0)

###############################
##### GRAFICAS Y TASAS DE LOS OBSTACULOS (POR TIPO)
###############################

def plotObstacleEvents(dataframe, title,obstacle, bar_color='blue',plot=True):
  spawnCount = getParameterCount(dataframe, "eventName", "OBSTACLE_SPAWN")
  dodgeCount = getParameterCount(dataframe, "eventName", "OBSTACLE_DODGE")
  collisionCount = getParameterCount(dataframe, "eventName", "OBSTACLE_COLLISION")

  print(f'Total dodged obstacles rate for {title}: {round((dodgeCount/spawnCount) * 100, 2) if spawnCount>0 else 100}% \n')
  if not plot:
    return

  graph = pd.DataFrame({'obstacle events': ['spawn', 'dodge', 'collision'], obstacle: [spawnCount, dodgeCount, collisionCount]})
  ax = graph.plot.bar(x='obstacle events', y=obstacle, rot=0,color=bar_color)

  # Añadir números encima de las barras
  for p in ax.patches:
      ax.annotate(f'{p.get_height()}', (p.get_x() + p.get_width() / 2., p.get_height()), ha='center', va='center', xytext=(0, 10), textcoords='offset points')

  plt.xlabel(f'Obstacle {obstacle} events')
  plt.ylabel('Count')
  plt.title(title)
  # plt.show()
  plt.savefig(title + '.png')

  print("\n\n")

def plotObstacleEventsByTypeStacked(dataframe,title,plot=True,palette='coolwarm'):
  obstacles = ['Bunny', 'BowlingPin', 'Ball', 'Ring']
  events = ['OBSTACLE_SPAWN', 'OBSTACLE_DODGE', 'OBSTACLE_COLLISION']
  colors = sns.color_palette(palette, len(obstacles))

  data = {obstacle: {event: dataframe[(dataframe['eventName'] == event) & (dataframe['obstacle'] == obstacle)].shape[0] for event in events} for obstacle in obstacles}

  graph = pd.DataFrame(data)
  graph.index = ['spawned', 'dodged', 'collided']

  sns.set()
  graph.plot(kind='bar', stacked=True, rot=0,color=colors)
  plt.xlabel('Obstacle events')
  plt.ylabel('Count')

  plt.title(title)
  # plt.show()
  plt.savefig(title + '.png')
  print("\n\n")

  # Graficar cada obstáculo por separado
  for obstacle in obstacles:
    title = (f'Obstacle events for {obstacle}')
    obstacle_df = dataframe[dataframe['obstacle'] == obstacle]
    plotObstacleEvents(obstacle_df,title,obstacle,colors[obstacles.index(obstacle)],plot)

def dodgeObstacleRate(spawnedObstacles, dodgedObstacles):
  rate = dodgedObstacles/spawnedObstacles
  return rate

spawnCount = getParameterCount(df, "eventName", "OBSTACLE_SPAWN")
dodgeCount = getParameterCount(df, "eventName", "OBSTACLE_DODGE")
print(f'Total dodged obstacles rate: {str((dodgeCount/spawnCount) * 100)}% \n')

plotObstacleEventsByTypeStacked(df,'Obstacle events in total')

###############################
##### MEDIA DE VIDAS PERDIDAS
###############################

lvlNum = getNumLevels(df)
collidedObstaclesNum = getParameterCount(df, 'eventName', 'OBSTACLE_COLLISION')

avgLivesLost = collidedObstaclesNum / lvlNum

print(f'Number of average lives lost: {avgLivesLost}')

###############################
##### PUNTUACION MEDIA
###############################

num_levels =getNumLevels(df)

# Puntuación en SONG_END
song_end_scores = df[df["eventName"] == "SONG_END"]["score"]
total_score = song_end_scores.sum()

# Cálculo de promedios
avg_score = total_score / num_levels if num_levels > 0 else 0

print("Average score:",avg_score)

###############################
##### TIEMPO DE VIDA MEDIO
###############################

# Tiempo jugado en SONG_END y PLAYER_DEATH
time_song_end = df[df["eventName"] == "SONG_END"]["secondsPlayed"]
time_deaths = df[df["eventName"] == "PLAYER_DEATH"]["secondsPlayed"]
total_time = pd.concat([time_song_end, time_deaths]).sum()

avg_time_alive = total_time / num_levels if num_levels > 0 else 0

print(f"Average time alive: {avg_time_alive} s")

###############################
##### GRAFICAS DE LOS OBSTACULOS POR TIPO AGRUPADAS POR FASE
###############################

def getEventsBetweenDifferentParameters(dataframe, parameter1, parameter2, firstPValue, secondPValue):
  # Buscar el primer y ultimo indice de la fila
  start_idx = dataframe[dataframe[parameter1] == firstPValue].index
  end_idx = dataframe[dataframe[parameter2] == secondPValue].index

  # Se juntan todas las filas entre ambos eventos
  data = pd.DataFrame()
  for i, j in zip(start_idx, end_idx):
      data = pd.concat([data, dataframe[i:j + 1]], ignore_index=True)

  return data

def collidedObstaclesPerPhase(dataframe):
  dfPhase0 = getEventsBetweenDifferentParameters(dataframe, 'eventName', 'newPhase', 'LEVEL_START', 1)
  dfPhase1 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'newPhase', 1, 2)
  dfPhase2 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'eventName', 3, 'LEVEL_END')

  collBunnyPhase0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase0['obstacle'] == 'Bunny')].shape[0]
  collBunnyPin0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase0['obstacle'] == 'BowlingPin')].shape[0]
  collBunnyBall0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase0['obstacle'] == 'Ball')].shape[0]
  collBunnyRing0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase0['obstacle'] == 'Ring')].shape[0]

  collBunnyPhase1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase1['obstacle'] == 'Bunny')].shape[0]
  collBunnyPin1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase1['obstacle'] == 'BowlingPin')].shape[0]
  collBunnyBall1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase1['obstacle'] == 'Ball')].shape[0]
  collBunnyRing1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase1['obstacle'] == 'Ring')].shape[0]

  collBunnyPhase2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase2['obstacle'] == 'Bunny')].shape[0]
  collBunnyPin2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase2['obstacle'] == 'BowlingPin')].shape[0]
  collBunnyBall2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase2['obstacle'] == 'Ball')].shape[0]
  collBunnyRing2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_COLLISION') & (dfPhase2['obstacle'] == 'Ring')].shape[0]

  g = {'Obstacles collided per phase': [0, 1, 2], 'Bunny': [collBunnyPhase0,collBunnyPhase1,collBunnyPhase2],  'BowlingPin': [collBunnyPin0,collBunnyPin1,collBunnyPin2], 'Ball': [collBunnyBall0,collBunnyBall1,collBunnyBall2], 'Ring': [collBunnyRing0,collBunnyRing1,collBunnyRing2]}
  graph= pd.DataFrame(data = g)

  sns.set()
  graph.set_index('Obstacles collided per phase').plot(kind='bar', rot=0, stacked=True)

def dodgedObstaclesPerPhase(dataframe):
  dfPhase0 = getEventsBetweenDifferentParameters(dataframe, 'eventName', 'newPhase', 'LEVEL_START', 1)
  dfPhase1 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'newPhase', 1, 2)
  dfPhase2 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'eventName', 3, 'LEVEL_END')

  dodgeBunnyPhase0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_DODGE') & (dfPhase0['obstacle'] == 'Bunny')].shape[0]
  dodgeBunnyPin0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_DODGE') & (dfPhase0['obstacle'] == 'BowlingPin')].shape[0]
  dodgeBunnyBall0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_DODGE') & (dfPhase0['obstacle'] == 'Ball')].shape[0]
  dodgeBunnyRing0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_DODGE') & (dfPhase0['obstacle'] == 'Ring')].shape[0]

  dodgeBunnyPhase1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_DODGE') & (dfPhase1['obstacle'] == 'Bunny')].shape[0]
  dodgeBunnyPin1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_DODGE') & (dfPhase1['obstacle'] == 'BowlingPin')].shape[0]
  dodgeBunnyBall1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_DODGE') & (dfPhase1['obstacle'] == 'Ball')].shape[0]
  dodgeBunnyRing1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_DODGE') & (dfPhase1['obstacle'] == 'Ring')].shape[0]

  dodgeBunnyPhase2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_DODGE') & (dfPhase2['obstacle'] == 'Bunny')].shape[0]
  dodgeBunnyPin2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_DODGE') & (dfPhase2['obstacle'] == 'BowlingPin')].shape[0]
  dodgeBunnyBall2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_DODGE') & (dfPhase2['obstacle'] == 'Ball')].shape[0]
  dodgeBunnyRing2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_DODGE') & (dfPhase2['obstacle'] == 'Ring')].shape[0]

  g = {'Obstacles dodged per phase': [0, 1, 2], 'Bunny': [dodgeBunnyPhase0,dodgeBunnyPhase1,dodgeBunnyPhase2],  'BowlingPin': [dodgeBunnyPin0,dodgeBunnyPin1,dodgeBunnyPin2], 'Ball': [dodgeBunnyBall0,dodgeBunnyBall1,dodgeBunnyBall2], 'Ring': [dodgeBunnyRing0,dodgeBunnyRing1,dodgeBunnyRing2]}
  graph= pd.DataFrame(data = g)

  sns.set()
  graph.set_index('Obstacles dodged per phase').plot(kind='bar', rot=0, stacked=True)

def spawnedObstaclesPerPhase(dataframe):
  dfPhase0 = getEventsBetweenDifferentParameters(dataframe, 'eventName', 'newPhase', 'LEVEL_START', 1)
  dfPhase1 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'newPhase', 1, 2)
  dfPhase2 = getEventsBetweenDifferentParameters(dataframe, 'newPhase', 'eventName', 3, 'LEVEL_END')

  spawnedBunnyPhase0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase0['obstacle'] == 'Bunny')].shape[0]
  spawnedBunnyPin0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase0['obstacle'] == 'BowlingPin')].shape[0]
  spawnedBunnyBall0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase0['obstacle'] == 'Ball')].shape[0]
  spawnedBunnyRing0 = dfPhase0[(dfPhase0['eventName'] == 'OBSTACLOBSTACLE_SPAWNE_DODGE') & (dfPhase0['obstacle'] == 'Ring')].shape[0]

  spawnedBunnyPhase1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase1['obstacle'] == 'Bunny')].shape[0]
  spawnedBunnyPin1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase1['obstacle'] == 'BowlingPin')].shape[0]
  spawnedBunnyBall1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase1['obstacle'] == 'Ball')].shape[0]
  spawnedBunnyRing1 = dfPhase1[(dfPhase1['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase1['obstacle'] == 'Ring')].shape[0]

  spawnedBunnyPhase2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase2['obstacle'] == 'Bunny')].shape[0]
  spawnedBunnyPin2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase2['obstacle'] == 'BowlingPin')].shape[0]
  spawnedBunnyBall2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase2['obstacle'] == 'Ball')].shape[0]
  spawnedBunnyRing2 = dfPhase2[(dfPhase2['eventName'] == 'OBSTACLE_SPAWN') & (dfPhase2['obstacle'] == 'Ring')].shape[0]

  g = {'Obstacles spawned per phase': [0, 1, 2], 'Bunny': [spawnedBunnyPhase0,spawnedBunnyPhase1,spawnedBunnyPhase2],  'BowlingPin': [spawnedBunnyPin0,spawnedBunnyPin1,spawnedBunnyPin2], 'Ball': [spawnedBunnyBall0,spawnedBunnyBall1,spawnedBunnyBall2], 'Ring': [spawnedBunnyRing0,spawnedBunnyRing1,spawnedBunnyRing2]}
  graph= pd.DataFrame(data = g)

  sns.set()
  graph.set_index('Obstacles spawned per phase').plot(kind='bar', rot=0, stacked=True)

collidedObstaclesPerPhase(df)
dodgedObstaclesPerPhase(df)
spawnedObstaclesPerPhase(df)

###############################
##### GRAFICAS DE LOS OBSTACULOS POR FASE
###############################

dfPhase0 = getEventsBetweenDifferentParameters(df, 'eventName', 'newPhase', 'LEVEL_START', 1)
dfPhase1 = getEventsBetweenDifferentParameters(df, 'newPhase', 'newPhase', 1, 2)
dfPhase2 = getEventsBetweenDifferentParameters(df, 'newPhase', 'eventName', 3, 'LEVEL_END')
plotObstacleEventsByTypeStacked(dfPhase0,'Obstacle events in phase 0',False)
plotObstacleEventsByTypeStacked(dfPhase1,'Obstacle events in phase 1',False)
plotObstacleEventsByTypeStacked(dfPhase2,'Obstacle events in phase 2',False)

###############################
##### TASAS DE SUPERACION Y ABANDONO
###############################

num_levels_started =getNumLevels(df)
num_levels_completed =getNumLevelsCompleted(df)
num_levels_quit = getNumLevelsQuit(df)

completion_rate = num_levels_completed / num_levels_started if num_levels_started > 0 else 0
abandon_rate = num_levels_quit / num_levels_started if num_levels_started > 0 else 0

print("Started levels:", num_levels_started)
print("Completed levels:", num_levels_completed)
print("Abandoned levels:", num_levels_quit)
print(f"Level completion rate: {completion_rate * 100}%")
print(f"Level abandonment rate: {abandon_rate * 100}%")

###############################
##### TIEMPO MEDIO EN CADA CARRIL
###############################

# Buscar el primer y ultimo indice de la fila
start_idx = df[df['eventName'] == 'LEVEL_START'].index
end_idx = df[df['eventName'] == 'LEVEL_END'].index

time = [0, 0, 0, 0, 0]

for i, j in zip(start_idx, end_idx):
    data = df[i:j + 1]

    currentTrack = 2
    lastTimestamp = 0

    for index, row in data.iterrows():
        if row['eventName'] == 'SONG_START':
            lastTimestamp = row['timestamp']
        elif row['eventName'] == 'PLAYER_MOVEMENT':
            if row['direction'] == 'LEFT' and currentTrack > 0:
                time[currentTrack] += (row['timestamp'] - lastTimestamp).total_seconds()
                lastTimestamp = row['timestamp']
                currentTrack -= 1
            elif row['direction'] == 'RIGHT' and currentTrack < 4:
                time[currentTrack] += (row['timestamp'] - lastTimestamp).total_seconds()
                lastTimestamp = row['timestamp']
                currentTrack += 1

time /= getNumLevels(df)

graph = pd.DataFrame({'Time per track': ['1', '2', '3', '4', '5'], 'time': time})
ax = graph.plot.bar(x='Time per track', y='time', rot=0)

plt.xlabel('Tracks')
plt.ylabel('Time')
title = 'Time per track'
plt.title(title)
# plt.show()
plt.savefig(title + '.png')

###############################
##### MEDIA DE MOVIMIENTOS REALIZADOS
###############################

def count_lane_changes(directions):
    changes = 0
    last_dir = None
    for dir in directions:
        if dir in ["LEFT", "RIGHT"]:
            if last_dir and dir != last_dir:
                changes += 1
            last_dir = dir
        else:
            last_dir = None
    return changes

def getEventsBetweenAny(dataframe, parameter, firstValue, lastValues):
    start_idx = dataframe[dataframe[parameter] == firstValue].index

    # Buscar el primer índice que tenga cualquiera de los valores en lastValues
    end_idx = dataframe[dataframe[parameter].isin(lastValues)].index

    data = pd.DataFrame()
    for i, j in zip(start_idx, end_idx):
        if i < j:
            data = pd.concat([data, dataframe[i:j + 1]], ignore_index=True)

    return data

def getMovementMetrics(dataframe):
    movements = dataframe[dataframe["eventName"] == "PLAYER_MOVEMENT"]
    directions = movements["direction"].value_counts()

    return {
        "LEFT": directions.get("LEFT", 0),
        "RIGHT": directions.get("RIGHT", 0),
        "UP": directions.get("UP", 0)
    }

nivel_df = getEventsBetweenAny(df, "eventName", "SONG_START", ["LEVEL_END", "LEVEL_QUIT"])


plt_color=sns.color_palette("coolwarm", 4)

metrics = getMovementMetrics(nivel_df)

# Calcular media por partida
avg_per_match = {k: (round(v / getNumLevels(df), 2) if getNumLevels(df) > 0 else 0) for k, v in metrics.items()}

avg_df = pd.DataFrame.from_dict(avg_per_match, orient="index")
plotBarChart(avg_df,'Average of each type of movement',"Average amount","Type of movement",plt_color[0])
