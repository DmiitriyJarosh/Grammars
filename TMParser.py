from collections import namedtuple

Transition = namedtuple("Transition", "startState endState read write direction")
State = namedtuple("State", "id")
Production = namedtuple("Production", "left right")
Automaton = namedtuple("Automaton", "states, transitions initialState finalStates")


def parseAutomaton(path):
    states = set()
    transitions = []
    with open(path, 'r') as f:
        for line in f:
            if line != '\n':
                line = line[:-1]
                splitted = line.split(',')
                trans = Transition(
                    startState=State(splitted[0]),
                    endState=State(splitted[2]),
                    read=splitted[1],
                    write=splitted[3],
                    direction=splitted[4]
                )
                states.add(State(splitted[0]))
                states.add(State(splitted[2]))
                transitions.append(trans)
        return Automaton(
            states=list(states),
            transitions=transitions,
            initialState=State("q1"),
            finalStates=[State("qF")]
        )
