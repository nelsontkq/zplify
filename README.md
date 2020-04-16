# Zplify

Allows for the creation of a ZPL label string out of an image file.  

Prints directly to the console unless -o or --output is provided, then it will save the zpl to the
target file.  

## Options

The full list of available options are as follows:  

-l --length    Set the length of the label in pixels. 1200px is default.  
-w --width     Set the width of the label in pixels. 800px is default.  
-o --output    Set the output path. If omitted, the label will output to the terminal  
-h --help      Display this message  

## Example usage

```shell
# send directly to zebra printer
./zplify ./path/to/file.png | lpr
```

```shell
# save to output.zpl and resize.
./zplify ./path/to/file.png -o output.zpl -l 1800 -w 800
```

```shell
# pipe input from URL, convert to ZPL and send to printer
wget -O www.example.com/label.png | ./zplify | lpr -P printer_name
```
