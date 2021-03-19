# Zplify - PNG to ZPL converter

Convert a PNG image to ZPL for a Zebra printer.

Prints the ZPL directly to the console unless -o or --output is provided, then it will save the zpl to the
target file.

## Options

The full list of available options are as follows:

```shell
-l --length             Set the length of the label in pixels. 1200px is default.  
-w --width              Set the width of the label in pixels. 800px is default.  
-o --output             Set the output path. If omitted, the label will output to the terminal 
-i --interpolation-mode Set the interpolation mode. 1-7, default is 7
                        1=Low, 2=High, 3=Bilinear, 4=Bicubic 5=NearestNeighbor 6=HighQualityBilinear 7=HighQualityBicubic
-h --help               Display this message  
```

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

## System requirements

Requires .net 5 runtime.

### macOS
App requires libgdiplus. You can install with brew:
```shell
brew install mono-libgdiplus
```
