import java.util.Arrays;

class Global{
public final String SCETCH_PATH = sketchPath();
public final String IMG_DIRECTORY = SCETCH_PATH + "/img/";
public final int TILESIZE = 64;
}


void setup() {
  Global g = new Global();
  
  // Using just the path of this sketch to demonstrate,
  // but you can list any directory you like.
  

  println("Listing all filenames in a directory: ");
  String[] filenames = listFileNames(g.IMG_DIRECTORY);
  for(int i = 0; i < filenames.length; i++)
    if(!filenames[i].endsWith(".png"))
      filenames = removeString(filenames, i);
  
  PImage[] images = new PImage[filenames.length];
    
  println("PNG-Images found:");
  
  PImage canvas = createImage(3 * filenames.length * g.TILESIZE, 3 * g.TILESIZE, ARGB);
  
  String[] struct = new String[filenames.length + 3];//First Line + int y + Brackets closed
  struct[0] = "struct TextureCords{";
  
  for(int i = 0; i < filenames.length; i++){
    struct[i + 1] = "public static final int " + filenames[i].toUpperCase().substring(0, filenames[i].length() - 4) + " = " + (i * 3 + 1) + ";";
    println("[" + i + "]" + filenames[i]);
    images[i] = loadImage(g.IMG_DIRECTORY + filenames[i]);
    addTile(canvas, images[i], i * 3 + 1, 1);
    
  }
  
  canvas.save("tilesheet.png");

  struct[struct.length - 2] = "public static final int y = 1;";
  struct[struct.length - 1] = "}";
  // Writes the strings to a file, each on a separate line
  saveStrings("struct.txt", struct);
  
  noLoop();
}

// Nothing is drawn in this program and the draw() doesn't loop because
// of the noLoop() in setup()
void draw() {
}

void addTile(PImage canvas, PImage tile, int x, int y){
  if(x < 1 || y < 1)
    return;
    
  insertTile(canvas, tile, x, y);
  
  PImage cp = tile.copy();
  
  flipH(tile);
  insertTile(canvas, tile, x - 1, y);
  insertTile(canvas, tile, x + 1, y);
  
  flipV(cp);
  insertTile(canvas, cp, x, y - 1);
  insertTile(canvas, cp, x, y + 1);
  
  flipV(tile);
  insertTile(canvas, tile, x - 1, y - 1);
  insertTile(canvas, tile, x + 1, y - 1);
  insertTile(canvas, tile, x - 1, y + 1);
  insertTile(canvas, tile, x + 1, y + 1);
}

// This function returns all the files in a directory as an array of Strings  
String[] listFileNames(String dir) {
  File file = new File(dir);
  if (file.isDirectory()) {
    String names[] = file.list();
    return names;
  } else {
    // If it's not a directory
    return null;
  }
}

String[] removeString(String[] arr, int rm){
  if(rm < 0)
    return new String[0];
    
  String[] ret = new String[arr.length - 1];
  
  int i = 0;
  
  for(; i < rm; i++)
    ret[i] = arr[i];
  
  for(; i < ret.length; i++)
    ret[i] = arr[i+1];
     
  return ret;
}

void insertTile(PImage canvas, PImage ins, int x, int y){
  Global g = new Global();
  
  for(int i = 0; i < ins.pixels.length; i++){
    int l = floor(i / g.TILESIZE) + y * g.TILESIZE;
    canvas.pixels[l * canvas.width + i % g.TILESIZE + x * g.TILESIZE] = ins.pixels[i];
  }
}

void flipHV(PImage img){
  int[] tmp = img.pixels;
  
  for(int i = 0; i < tmp.length; i++)
    img.pixels[i] = tmp[tmp.length - i - 1];
}

void flipH(PImage img){
  int[] tmp = img.pixels.clone();
  
  println(img.pixels.length);
  
  for(int y = 0; y < img.height * img.width; y += img.width){
    for(int x = 0; x < img.width; x++){
      img.pixels[y + x] = tmp[y + img.width - x - 1];
    }
  }
    
}

void flipV(PImage img){
  int[] tmp = img.pixels.clone();
  
  for(int y = 0; y < tmp.length; y += img.width){
    for(int x = 0; x < img.width; x++){
      img.pixels[y + x] = tmp[tmp.length - img.width - y + x];
    }
  }
  
    
}
