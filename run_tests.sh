#!/bin/bash

program=`realpath ./MiniCompiler/bin/Debug/MiniCompiler.exe`
tests=`echo ./TestCases/*/`

function testValidCases {
	files=$1;
	for file in $files
	do
		[ -f "$file" ] || break
		
		$program $file
	done
}

function testWrongCases {
	echo ""
}

for dir in $tests # trailing slash takes only directories
do
	echo "-------------------------------------------------------------------------------------"
    dir=${dir%*/}      # remove the trailing "/"
    echo -e ${dir##*/}: "\n"    # print everything after the final "/"
	
	dir=`realpath $dir`
	files=`echo $dir/*.txt`
	
	if [[ "$dir" == *Valid ]]; then
		testValidCases "$files"
	elif [[ "$dir" == *Wrong ]]; then
		testWrongCases "$files"
	else
		echo "nothing"
	fi
done
