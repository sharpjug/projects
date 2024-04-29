import random
from table import Board

# OBJECTS
board = Board()

# Start the game
board.start_game()
print("Welcome to console hangman!")

while True:
    # Pass a guess to the board
    guess = input("New guess: ")
    board.guess(guess)

    # If the game has been lost or won, call end function
    if board.game_end:
        board.end()
        # If user wants to restart, restart
        if board.restart:
            board.start_game()


