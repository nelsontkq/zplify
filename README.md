# zplify

Allows for the creation of a ZPL label parsable by a zebra printer out of an image file.  

Prints directly to the console unless -o or --output is provided, then it will save the zpl to the target file.  

## Options

The full list of available options are as follows:  

-l --length    Set the length of the label in pixels. 1200px is default.  
-w --width     Set the width of the label in pixels. 800px is default.  
-o --output    Set the output path. If omitted, the label will output to the terminal  
-h --help      Display this message  
