# Turing machine interpreter

import sys
from collections import defaultdict


def start_tm(word):
    tape = []
    for i in word:
        tape.append(i)

    print(tape)
    tape = list(tape)
    tmDelta = defaultdict(tuple)

    with open('TM.txt', 'r') as f:
        for line in f:
            if line != '\n':
                line = line[:-1]
                splitted = line.split(',')
                tmDelta[(splitted[0], splitted[1])] = splitted[2:]

    pos = 0
    state = 'q1'
    tmp = tape
    tmp = tmp[:pos] + ['>'] + tmp[pos:]
    print(' '.join(tmp) + ' ' + state)

    while True:
        nextState = tmDelta[(state, tape[pos])]

        if nextState == ():
            break

        state = nextState[0]
        tape[pos] = nextState[1]
        pos = pos + 1 if nextState[2] == 'r' else pos - 1

        if pos < 0:
            tape.insert(0, '_')
            pos = 0

        try:
            gotdata = tape[pos]
        except IndexError:
            tape.insert(pos, '_')

        tmp = tape
        tmp = tmp[:pos] + ['>'] + tmp[pos:]
        print(' '.join(tmp) + ' ' + state)

    return state


if __name__ == '__main__':
    state = start_tm(sys.argv[1])
    if state == 'qF':
        print("Input is prime number!")
    else:
        print("Input isn't prime number!")
