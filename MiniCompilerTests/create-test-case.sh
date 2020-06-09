#!/bin/bash
set -e;

if [[ ! $2 ]]; then 
    echo "Error"
	exit 1;
fi;

echo $1;
echo $2;

IFS='.' read -ra ARR <<< "$1"
path=".";
goBack=".";
# Create directory structure for provided namespace
for dir in "${ARR[@]}"; do
	mkdir -p ./$dir;
	cd ./$dir;
	path="$path/$dir";
	goBack="$goBack/..";
done

cd $goBack;
echo $path;
resultFolder="$path/${2}Tests";
cp -r TemplateTests "$resultFolder";
echo $resultFolder
cd "$resultFolder";

if [[ $1 ]]; then 
    # Add namespace
	sed -i "s/MiniCompilerTests/MiniCompilerTests.$1/g" TemplateValidTests.cs
	sed -i "s/MiniCompilerTests/MiniCompilerTests.$1/g" TemplateNotValidTests.cs
	echo "X"
	# Change base class
	sed -i "s/: ValidTests/: ${ARR[-1]}ValidTests/g" TemplateValidTests.cs
	sed -i "s/: NotValidTests/: ${ARR[-1]}NotValidTests/g" TemplateNotValidTests.cs
fi
echo "X"
# Change class names
sed -i "s/TemplateValidTests/${2}ValidTests/g" TemplateValidTests.cs
sed -i "s/TemplateNotValidTests/${2}NotValidTests/g" TemplateNotValidTests.cs

# Change file names
mv TemplateValidTests.cs "${2}ValidTests.cs"
mv TemplateNotValidTests.cs "${2}NotValidTests.cs"
