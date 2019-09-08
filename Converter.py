from TMParser import parseAutomaton, Production

myAllReadableString = set()
myAllWritableString = set()

SQUARE_SYMBOL = '_'
SQUARE = "="
VAR_START = "V("
VAR_END = ")"


def fillMap(automaton):
    objectToProduction = {}
    productionToObject = {}
    states = automaton.states
    for i in range(0, len(states)):
        prods = getProductionsFromState(states[i], automaton)
        if len(prods) == 0:
            continue
        objectToProduction[states[i]] = prods
        for j in range(0, len(prods)):
            productionToObject[prods[j]] = states[i]

    transitions = automaton.transitions
    for i in range(0, len(transitions)):
        prods = getProductionsFromTransition(transitions[i], automaton)
        if len(prods) == 0:
            continue
        objectToProduction[transitions[i]] = prods
        for j in range(0, len(prods)):
            productionToObject[prods[j]] = transitions[i]

    return objectToProduction


def concatProd(automaton):
    res = []
    list = fillMap(automaton)
    for (key, value) in list.items():
        res.extend(value)
    return res


def getProductionsFromState(state, automaton):
    if automaton.initialState == state:
        tm = automaton.transitions
        return createProductionsForInit(state, tm)
    return []


def getProductionsFromTransition(transition, automaton):
    return createProductionsForTransition(transition, automaton.finalStates)


def createProductionsForInit(state, tm):
    id = state.id
    init = [
        Production("S", VAR_START + SQUARE + SQUARE + VAR_END + "S"),
        Production("S", "S" + VAR_START + SQUARE + SQUARE + VAR_END),
        Production("S", "T")
    ]

    myAllReadableString.add(SQUARE)

    for i in range(0, len(tm)):
        trans = tm[i]
        tape = 1
        for j in range(0, tape):
            str = trans.read
            if str == SQUARE_SYMBOL:
                str = SQUARE
            write = trans.write
            if write == SQUARE_SYMBOL:
                write = SQUARE
            myAllWritableString.add(write)
            if str not in myAllReadableString:
                myAllReadableString.add(str)
                var1 = VAR_START + str + str + VAR_END
                var2 = VAR_START + str + id + str + VAR_END
                init.append(Production("T", "T" + var1))
                init.append(Production("T", var2))

    init.append(Production(SQUARE, "EmptySymbol"))

    return init.copy()


def createProductionsForTransition(transition, states):
    list = []
    trans = transition
    finalStateMap = set()
    for i in range(0, len(states)):
        finalStateMap.add(states[i].id)

    fromState = trans.startState.id
    toState = trans.endState.id
    direction = trans.direction
    read = trans.read
    write = trans.write
    if read == SQUARE_SYMBOL:
        read = SQUARE
    if write == SQUARE_SYMBOL:
        write = SQUARE

    for p in myAllReadableString:
        for a in myAllReadableString:
            for q in myAllWritableString:
                if direction == "r":
                    lhs_var1 = VAR_START + a + fromState + read + VAR_END
                    lhs_var2 = VAR_START + p + q + VAR_END
                    rhs_var1 = VAR_START + a + write + VAR_END
                    rhs_var2 = VAR_START + p + toState + q + VAR_END
                    prod = Production(lhs_var1 + lhs_var2, rhs_var1 + rhs_var2)
                    list.append(prod)

                    if toState in finalStateMap:
                        lhs = VAR_START + p + toState + q + VAR_END
                        rhs = p
                        list.append(Production(lhs, rhs))
                        lhs2 = VAR_START + a + q + VAR_END + p
                        list.append(Production(lhs2, a + rhs))
                        lhs3 = p + VAR_START + a + q + VAR_END
                        list.append(Production(lhs3, p + a))

                if direction == "l":
                    lhs_var1 = VAR_START + p + q + VAR_END
                    lhs_var2 = VAR_START + a + fromState + read + VAR_END

                    rhs_var1 = VAR_START + p + toState + q + VAR_END
                    rhs_var2 = VAR_START + a + write + VAR_END

                    prod = Production(lhs_var1 + lhs_var2, rhs_var1 + rhs_var2)

                    list.append(prod)

                    if toState in finalStateMap:
                        lhs = VAR_START + p + toState + q + VAR_END
                        rhs = p
                        lhs2 = p + VAR_START + a + q + VAR_END
                        list.append(Production(lhs, rhs))
                        list.append(Production(lhs2, p + a))

                        lhs3 = VAR_START + a + q + VAR_END + p
                        list.append(Production(lhs3, a + rhs))

    return list.copy()


if __name__ == '__main__':
    automaton = parseAutomaton("TM.txt")
    with open("output.txt", 'w') as f:
        for t in concatProd(automaton):
            f.write("{0} -> {1}\n".format(t.left, t.right))
