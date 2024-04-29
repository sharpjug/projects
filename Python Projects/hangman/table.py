import random
import sys


class Board:
    def __init__(self):
        self.columns = 9
        self.rows = 5
        self.filler = '-'
        self.lives_lost = 0
        self.word = ""
        self.guessed = []
        self.hidden_word = ""
        self.letters_left = 0
        self.game_end = False
        self.restart = False
        self.first_playthrough = True

    def check_guess(self, guess):
        # Checks if a guess has been previously made
        if guess in self.guessed:
            print("This has been already guessed, try again!")
            return False
        else:
            return True

    def guess(self, guess):
        guess_text = ""
        # Check if valid guess and if in the word
        if self.check_guess(guess):
            self.guessed.append(guess)
            if guess in self.word:
                # replace letter with hidden text
                i = 0
                temp_list = list(self.hidden_word)
                # Track the new letter discovered
                for letter in self.word:
                    if guess == letter:
                        temp_list[i] = letter
                        self.letters_left -= 1

                    i += 1
                self.hidden_word = ''.join(temp_list)
                guess_text = "Correct Guess!"
            else:
                self.lives_lost += 1
                guess_text = "Wrong Guess!"
        # Reshow the updated table
        self.clear_some_space()
        self.create_table()
        print(guess_text)
        self.show_word()
        print("Guessed letters:" +  ','.join(self.guessed))
        if self.letters_left == 0 or self.lives_lost == 7:
            self.game_end = True

    # Move console text further down
    def clear_some_space(self):
        for i in range(0,3):
            print("")

    def get_random_word(self):
        # Get random word from the text file and then break it up into usable chars
        with open("words.txt", "r") as file:
            words = file.read().split("\n")
            self.word = random.choice(words)
        self.breakup_word()

    def end(self):
        # If won or lost, displays that and calls upon another game
        self.first_playthrough = False
        if self.lives_lost == 7:
            print("You have lost. Word was: " + ''.join(self.word))
        else:
            print("You win!")
        restart = input("Would you like to play again? Please enter y if so!\n")
        if restart == 'n':
            sys.exit()
        if restart == 'y':
            self.restart = True

    def start_game(self):
        # Reset of all variables and getting data
        self.lives_lost = 0
        self.guessed = []
        self.game_end = False
        self.restart = False
        if not self.first_playthrough:
            self.clear_some_space()
        self.get_random_word()
        self.word_shown_fresh()
        self.create_table()
        self.show_word()

    def word_shown_fresh(self):
        # Shows a new word in *
        self.hidden_word = ""
        self.letters_left = len(self.word)
        for word in self.word:
            self.hidden_word += '*'

    def show_word(self):
        print("Word so far is : " + self.hidden_word)

    def breakup_word(self):
        # Breaks up the main word into an array
        temp_arr = []
        for letter in self.word:
            temp_arr.append(letter)
        self.word = temp_arr

    def create_table(self):
        # Creates a component depending on lives lost and where it is within the table
        row = ""
        for rows in range(0,self.rows):
            for col in range(0, self.columns):
                if self.lives_lost > 0 and rows == 0 and col == 4:
                    row += '|'
                    continue
                if self.lives_lost > 1 and rows == 1 and col == 4:
                    row += 'O'
                    continue
                if self.lives_lost > 2 and rows == 2 and col == 4:
                    row += '|'
                    continue
                if self.lives_lost > 3 and rows == 2 and col == 3:
                    row += '/'
                    continue
                if self.lives_lost > 4 and rows == 2 and col == 5:
                    row += '\\'
                    continue
                if self.lives_lost > 5 and rows == 3 and col == 3:
                    row += '/'
                    continue
                if self.lives_lost > 6 and rows == 3 and col == 5:
                    row += '\\'
                    continue
                row += self.filler
            print(row)
            row = ""