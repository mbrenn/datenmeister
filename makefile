SHELL = /bin/sh

SOURCE_FILES = src/js/dejs.gallery.ts src/js/dejs.string.ts src/js/dejs.table.ts src/js/dejs.number.ts

all: bin/js/dejs/dejs.ajax.js \
	bin/js/dejs/dejs.gallery.js \
	bin/js/dejs/dejs.string.js \
	bin/js/dejs/dejs.table.js \
	bin/js/dejs/dejs.number.js
	
	mkdir -p bin
	mkdir -p bin/js/jquery
	mkdir -p bin/js/dejs
	mkdir -p bin/js/init
	mkdir -p bin/tests

	cp src/index.html bin/index.html
	cp -r src/js/* bin/js/
	cp -r src/tests/* bin/tests/

# Rule to transfer Typescript files to JavaScript files
bin/js/dejs/%.js : src/js/dejs/%.ts
	mkdir -p bin/js/jquery
	cp src/js/jquery/jquery.d.ts bin/js/jquery/

	mkdir -p bin/js/dejs
	cp $< bin/js/dejs
	tsc bin/js/dejs/$(<F) --module amd --declaration --sourcemap


.PHONY: clean
clean:
	rm -rf bin

run: all
	firefox http://127.0.0.1:8080/ & 
	xsp2 --root $(abspath bin/)

