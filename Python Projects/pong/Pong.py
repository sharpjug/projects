import pygame, sys
from Ball import Ball
from Paddle import Paddle
from Settings import *

pygame.init()

screen = pygame.display.set_mode((WIDTH,HEIGHT))
pygame.display.set_caption("Ping Pong")

#Variables
playing = False
FPS = pygame.time.Clock()

class Collision:
    def ball_and_paddle1(self, ball, paddleA):
        if ball.posY + ball.radius > paddleA.posY and ball.posY - ball.radius < paddleA.posY + paddleA.height:
            if ball.posX - ball.radius <= paddleA.posX + paddleA.width:
                ball.slow_down = 1
                return True
        return False
    def ball_and_paddle2(self, ball, paddleB):
        if ball.posY + ball.radius > paddleB.posY and ball.posY - ball.radius < paddleB.posY + paddleB.height:
            if ball.posX - ball.radius >= paddleB.posX - paddleB.width:
                ball.slow_down = 1
                return True
        return False

    def ball_and_walls(self, ball):
        #Top
        if ball.posY - ball.radius <= 0:
            return True
        #Bottom
        if ball.posY + ball.radius >= HEIGHT:
            return True
        return  False

class Score:
    def __init__(self, screen, paddleA, paddleB, ball):
        self.ball = ball
        self.playerA = paddleA
        self.playerB = paddleB
        self.screen = screen
        self.font = pygame.font.SysFont('Bauhaus 93', 60)
        self.inst_font = pygame.font.SysFont('Bauhaus 93', 30)
        self.color = pygame.Color("white")
        self.End = False

    def scoreboard(self):
        A_score, B_score = str(self.playerA.score), str(self.playerB.score)
        A_score = self.font.render(A_score, True, self.color)
        B_score = self.font.render(B_score, True, self.color)
        self.screen.blit(A_score, (WIDTH // 4, 50))
        self.screen.blit(B_score, ((WIDTH // 4) * 3, 50))

    def ball_out(self):
        # Ball that scores a point
        if self.ball.posX >= WIDTH - 10:
            paddleA.score += 1
            if paddleA.score == 9:
                self.match("Player A")
            self.ball.direction = 'right'
            self.ball.slow_down = 4
            self.ball.reset_position()
            self.ball.start_moving()
        elif self.ball.posX <= 7:
            paddleB.score += 1
            if paddleB.score == 9:
                self.match("Player B")
            self.ball.direction = 'left'
            self.ball.slow_down = 4
            self.ball.reset_position()
            self.ball.start_moving()

    def match(self, winner):
        text = "Winner is " + winner + "!"
        self.End = True
        text = self.font.render(text, True, self.color)
        paint_black()
        self.screen.blit(text, (WIDTH // 4, HEIGHT/2 - 50))


def paint_black():
    screen.fill(BLACK)
    pygame.draw.line(screen,WHITE,(WIDTH//2, 0),(WIDTH//2, HEIGHT), 2)

paint_black()

# OBJECTS
ball = Ball(screen, WHITE, WIDTH//2, HEIGHT//2, 12)
ball.save_position()
paddleHeight, paddleWidth = 80, 20
paddleA = Paddle(screen, WHITE, 15, HEIGHT//2 - (paddleHeight/2), 20, paddleHeight)
paddleB = Paddle(screen, WHITE, WIDTH - paddleWidth - 15, HEIGHT//2 - (paddleHeight/2), 20, paddleHeight)
collision = Collision()
score = Score(screen, paddleA, paddleB, ball)

# Main Loop
while True:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            sys.exit()
        if event.type == pygame.KEYDOWN and score.End:
            pygame.quit()
            sys.exit()
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_p:
                ball.start_moving()
                playing = True
            if event.key == pygame.K_UP:
                paddleB.state = 'up'
            elif event.key == pygame.K_DOWN:
                paddleB.state = 'down'
            if event.key == pygame.K_w:
                paddleA.state = 'up'
            elif event.key == pygame.K_s:
                paddleA.state = 'down'
        elif event.type == pygame.KEYUP:
            paddleA.state = 'stopped'
            paddleB.state = 'stopped'
    if playing:
        paint_black()

        # Ball movement
        ball.move()
        ball.draw()

        # PlayerA
        paddleA.move()
        paddleA.set_boundaries()
        paddleA.draw()

        #PlayerB
        paddleB.move()
        paddleB.set_boundaries()
        paddleB.draw()

        # Collisions
        if collision.ball_and_paddle1(ball, paddleA):
            ball.paddle_collision()
        elif collision.ball_and_paddle2(ball, paddleB):
            ball.paddle_collision()
        elif collision.ball_and_walls(ball):
            ball.wall_collision()
    FPS.tick(60)

    # Scoring ball and loading scoreboard
    score.ball_out()
    score.scoreboard()
    if score.End:
        playing = False
    pygame.display.update()