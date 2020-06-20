#!/bin/bash

main="../MiniCompilerComplete/Main.cs";

echo "using QUT.Gppg;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Reflection;
using System.Collections;
using System.Text;

namespace MiniCompilerComplete
{" > $main;

for file in `find . -name '*.cs' | grep -v -e "Parser.cs" -e "Scanner.cs" -e "bin/" -e "obj/" -e "Properties/" `; do
    i=0;
    write=0;
    echo $file;
    while IFS= read -r line; do
        if [[ $line == *"namespace"* ]]; then
            write=$write+1; 
        elif [[ $write -eq 1 && $line == *"{"* ]]; then
            write=$write+1;
        elif [[ $write -eq 2 ]]; then
            echo "$line";
            echo "$line" >> $main
        fi
    done < $file
    echo "" >> $main
done

echo "}" >> $main;
