# A template C++ Makefile for your SAT solver.

# Debugging flags
#FLAGS=-Wall -Wold-style-cast -Wformat=2 -ansi -pedantic -ggdb3 -DDEBUG

# Optimizing flags
FLAGS=-Wall -Wold-style-cast -Wformat=2 -std=c++14 -pedantic -g -O3

# List all the .o files you need to build here
OBJS=parser.o SatSolver.o

# This is the name of the executable file that gets built.  Please
# don't change it.
EXENAME=yasat
CC=g++

# Compile targets
all: $(OBJS)
	$(CC) $(FLAGS) $(OBJS) -o $(EXENAME)
parser.o: parser.cpp parser.h
	$(CC) $(FLAGS) -c parser.cpp
SatSolver.o: SatSolver.cpp parser.h
	$(CC) $(FLAGS) -c SatSolver.cpp

# Add more compilation targets here



# The "phony" `clean' compilation target.  Type `make clean' to remove
# your object files and your executable.
.PHONY: clean
clean:
	rm -rf $(OBJS) $(EXENAME)
