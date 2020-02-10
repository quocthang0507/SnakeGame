# https://www.edureka.co/blog/snake-game-with-pygame/
import pygame
import sys
import time
import random

pygame.init()

# colors
white = (255, 255, 255)
yellow = (255, 255, 102)
black = (0, 0, 0)
red = (213, 50, 80)
green = (0, 255, 0)
blue = (50, 153, 213)
# window size
width = 600
height = 400
# window's property
dis = pygame.display.set_mode((width, height))
pygame.display.set_caption('Snake Game by Edureka')
# time
clock = pygame.time.Clock()
# snake's property
snake_block = 10
snake_speed = 10
# font style
font_style = pygame.font.SysFont("bahnschrift", 25)
score_font = pygame.font.SysFont("comicsansms", 30)


def print_score(score):
    value = score_font.render("Score: " + str(score), True, yellow)
    dis.blit(value, [0, 0])


def print_snake(snake_block, snake_list):
    for x in snake_list:
        pygame.draw.rect(dis, black, [x[0], x[1], snake_block, snake_block])


def print_message(msg, color):
    mesg = font_style.render(msg, True, color)
    dis.blit(mesg, [width / 6, height / 3])


def play():
    game_over = False
    game_close = False

    x1 = width / 2
    y1 = height / 2

    x1_change = 0
    y1_change = 0

    snake_list = []
    snake_len = 1

    fx = round(random.randrange(0, width - snake_block) / 10.0) * 10.0
    fy = round(random.randrange(0, height - snake_block) / 10.0) * 10.0

    while not game_over:

        while game_close == True:
            dis.fill(blue)
            print_message("You Lost! Press C-Play Again or Q-Quit", red)
            print_score(snake_len - 1)
            pygame.display.update()

            for event in pygame.event.get():
                if event.type == pygame.KEYDOWN:
                    if event.key == pygame.K_q:
                        game_over = True
                        game_close = False
                    if event.key == pygame.K_c:
                        play()

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                game_over = True
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_LEFT:
                    x1_change = -snake_block
                    y1_change = 0
                elif event.key == pygame.K_RIGHT:
                    x1_change = snake_block
                    y1_change = 0
                elif event.key == pygame.K_UP:
                    y1_change = -snake_block
                    x1_change = 0
                elif event.key == pygame.K_DOWN:
                    y1_change = snake_block
                    x1_change = 0

        if x1 >= width or x1 < 0 or y1 >= height or y1 < 0:
            game_close = True
        x1 += x1_change
        y1 += y1_change
        dis.fill(blue)
        pygame.draw.rect(dis, green, [fx, fy, snake_block, snake_block])
        head = []
        head.append(x1)
        head.append(y1)
        snake_list.append(head)
        if len(snake_list) > snake_len:
            del snake_list[0]

        for x in snake_list[:-1]:
            if x == head:
                game_close = True

        print_snake(snake_block, snake_list)
        print_score(snake_len - 1)

        pygame.display.update()

        if x1 == fx and y1 == fy:
            fx = round(random.randrange(
                0, width - snake_block) / 10.0) * 10.0
            fy = round(random.randrange(
                0, height - snake_block) / 10.0) * 10.0
            snake_len += 1

        clock.tick(snake_speed)

    pygame.quit()
    quit()


def main():
    play()

if __name__ == "__main__":
    sys.exit(int(main() or 0))
