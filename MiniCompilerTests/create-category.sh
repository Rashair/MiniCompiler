#!/bin/bash
set -e;

if [[ ! $1 ]]; then 
    echo "Error"
	exit 1;
fi;

echo $1;
path=`realpath ./Template`;
echo $path;

IFS='.' read -ra ARR <<< "$1"
namespace=""
prev=""
# Create directory structure for provided namespace
for dir in "${ARR[@]}"; do
	if [[ ! $namespace ]]; then
		namespace=$dir
	else
		namespace="$namespace.$dir";
	fi
	
	if [ ! -d "./$dir" ]; then
		cp -r $path ./$dir
		cd $dir
		# Add namespace
		sed -i "s/MiniCompilerTests/MiniCompilerTests.${namespace}/g" TemplateValidTests.cs
		sed -i "s/MiniCompilerTests/MiniCompilerTests.${namespace}/g" TemplateNotValidTests.cs
		
		# Change base class
		sed -i "s/: ValidTests/: ${prev}ValidTests/g" TemplateValidTests.cs
		sed -i "s/: NotValidTests/: ${prev}NotValidTests/g" TemplateNotValidTests.cs
		
		# Change class names and suffix
		sed -i "s/Template/${dir}/g" TemplateValidTests.cs
		sed -i "s/Template/${dir}/g" TemplateNotValidTests.cs
		
		# Change file names
		mv TemplateValidTests.cs "${dir}ValidTests.cs"
		mv TemplateNotValidTests.cs "${dir}NotValidTests.cs"
	else
		cd $dir;
	fi
	prev=$dir
done
