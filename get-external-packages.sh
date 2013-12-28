#!/bin/bash

mkdir -p ext-packages

cd ext-packages

# Loading DefinitelyTyped
if [ ! -d DefinitelyTyped ]
then
	git clone https://github.com/borisyankov/DefinitelyTyped
fi 

cd DefinitelyTyped
git checkout ea3efa8377050a0464a21a88bd3251c6f34938b8
cd ..

# Loading bootstrap
if [ ! -d bootstrap ]
then
	git clone https://github.com/twbs/bootstrap.git
fi
cd bootstrap
	git checkout v3.0.3
cd ..

# Loading underscore
if [ ! -d underscore ]
then
	git clone https://github.com/jashkenas/underscore.git
fi

cd underscore
	git checkout 1.5.2
cd ..

# Loading backbone
if [ ! -d backbone ]
then
	git clone https://github.com/jashkenas/backbone.git
fi
cd backbone
	git checkout 1.1.0
cd ..

cd ..

