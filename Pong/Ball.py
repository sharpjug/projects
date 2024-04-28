import pygame, random

class Ball:
    def __init__(self, screen, color, posX, posY, radius):
        self.screen = screen
        self.color = color
        self.posX = posX
        self.posY = posY
        self.radius = radius
        self.dx = 0
        self.dy = 0
        self.speed = 80
        self.rect = pygame.Rect(posX,posY,radius, radius)
        self.resetposX = 0
        self.resetposY = 0
        self.Stop = False
        self.slow_down = 1
        self.direction = None

    def start_moving(self):
        self.dx = 0.15 * self.speed / self.slow_down
        self.dy = 0.05 * self.speed / self.slow_down
        self.random_direction()

    def random_direction(self):
        if self.direction == 'right':
            if self.dx < 0:
                self.dx = -self.dx
        elif self.direction == 'left':
            if self.dx > 0:
                self.dx = -self.dx
        else:
            self.dx = random.choice([self.dx, -self.dx])
        self.dy = random.choice([self.dy, -self.dy])
        self.direction = None

    def move(self):
        if self.Stop == False:
            self.posX += self.dx
            self.posY += self.dy

    def paddle_collision(self):
        if self.dx > 0:
            self.dx = -(0.15 * self.speed / self.slow_down)
        else:
            self.dx = 0.15 * self.speed / self.slow_down
        self.slow_down = 1

    def wall_collision(self):
        self.dy = -self.dy

    def reset_position(self):
        self.posX = self.resetposX
        self.posY = self.resetposY

    def save_position(self):
        self.resetposX = self.posX
        self.resetposY = self.posY

    def Ball_Full_Stop(self):
        self.dx = 0
        self.dy = 0
        self.Stop = True

    def draw(self):
        pygame.draw.circle(self.screen, self.color, (self.posX,self.posY), self.radius)