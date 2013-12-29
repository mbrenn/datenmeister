SHELL = /bin/sh

SOURCE_FILES = src/js/dejs.gallery.ts src/js/dejs.string.ts src/js/dejs.table.ts src/js/dejs.number.ts

all: bin/js/dejs.ajax.js bin/js/dejs.gallery.js bin/js/dejs.string.js bin/js/dejs.table.js bin/js/dejs.number.js
	mkdir -p bin
	mkdir -p bin/js
	mkdir -p bin/js/lib
	mkdir -p bin/js/init
	mkdir -p bin/tests

	cp src/index.html bin/index.html
	cp -r src/js/init/* bin/js/init/
	cp -r src/js/lib/* bin/js/lib/
	cp -r src/tests/* bin/tests/

# Rule to transfer Typescript files to JavaScript files
bin/js/%.js : src/js/%.ts
	mkdir -p bin/js
	cp $< bin/js
	tsc bin/js/$(<F) --module amd --declaration --sourcemap

bin/js/jquery.d.ts: src/js/lib/jquery.d.ts
	mkdir -p bin/js
	cp $< bin/js

bin/js/dejs.ajax.js: src/js/dejs.ajax.ts bin/js/jquery.d.ts

bin/js/dejs.gallery.js: src/js/dejs.gallery.ts

bin/js/dejs.string.js: src/js/dejs.string.ts

bin/js/dejs.table.js: src/js/dejs.table.ts

bin/js/dejs.number.js: src/js/dejs.number.ts

.PHONY: clean
clean:
	rm -rf bin

run: all
	firefox http://127.0.0.1:8080/ & 
	xsp2 --root $(abspath bin/)

