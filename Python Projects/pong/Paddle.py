import pygame
from Settings import *

class Paddle:
    def __init__(self, screen, color, posX, posY, width, height):
        self.screen = screen
        self.color = color
        self.posX = posX
        self.posY = posY
        self.width = width
        self.height = height
        self.state = 'stopped'
        self.score = 0
        self.rect = pygame.Rect(self.posX,self.posY, self.width,self.height)

    def move(self):
        if self.state == 'up':
            self.posY -= 10
        elif self.state == 'down':
            self.posY += 10

    def draw(self):
        pygame.draw.rect(self.screen, self.color, (self.posX, self.posY, self.width,self.height))

    def set_boundaries(self):
        if self.posY <= 0:
            self.posY = 0
        elif self.posY >= HEIGHT - self.height:
            self.posY = HEIGHT - self.height