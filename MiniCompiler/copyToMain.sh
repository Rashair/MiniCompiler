#!/bin/bash

for file in `find . -name '*.cs' | grep -v -e "Parser.cs" -e "Scanner.cs" -e "bin/" -e "obj/" -e "Properties/" `; do
    i=0;
    write=0;
    while IFS= read -r line; do
        echo $line;
        if [[ $line == *"namespace"* ]]; then
            write=$write+1; 
        elif [[ $write -eq 1 ]]; then
            write=$write+1;
        elif [[ $write -eq 2 ]]; then
            echo $line;
        fi
    done < $file
    echo ""
    echo $file;
done
