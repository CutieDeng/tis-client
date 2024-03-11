from PIL import Image 
import sys

args = sys.argv 
if len(args) != 2: 
    print("Usage: python flipColor.py <image file>")
    sys.exit(1) 

image = Image.open(args[1]) 
image = image.convert("RGBA") 
data = image.getdata() 

newData = [] 
for item in data: 
    newI = [0, 0, 0, 0] 
    newI[0] = 255 - item[0] 
    newI[1] = 255 - item[1]
    newI[2] = 255 - item[2] 
    newI[3] = item[3] 

    newData.append(tuple(newI)) 

image.putdata(newData)
image.save("outFlipColor.png", "PNG") 
    