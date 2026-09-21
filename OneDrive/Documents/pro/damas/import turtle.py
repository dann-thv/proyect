import turtle
import math

screen = turtle.Screen()
screen.bgcolor("black")

t = turtle.Turtle()

t.speed(1)
t.hideturtle()
t.penup()
t.color("#0854F8")

for scale in range(11, 17):
    for i in range(120):
      angle = i * (math.pi * 2) / 120

      x = 16 * (math.sin(angle) ** 3)* scale
      y = (13 * math.cos(angle) - 5 *
 math.cos(2 * angle) - 2 * math.cos(3 *
 angle) - math.cos(4 * angle)) * scale

      t.goto(x, y)
      t.write("te amo", align="center", font=("arial", 8, "bold"))
turtle.done()