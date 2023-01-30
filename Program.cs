using Raylib_cs;

namespace FallingSand
{
    static class Program
    {

        static int SCREENWIDTH = 500;
        static int SCREENHEIGHT = 500;

        static int GRIDWIDTH = 500;
        static int GRIDHEIGHT = 500;

        static int PIXELWIDTH = SCREENWIDTH/GRIDWIDTH;
        static int PIXELHEIGHT = SCREENHEIGHT/GRIDWIDTH;





        public static void Main()
        {
            int[,] Map = new int[GRIDWIDTH,GRIDHEIGHT];
            Stack<Pixel> pixelReferences = new Stack<Pixel>();
            int pixelCount = 0;
            Raylib.InitWindow(SCREENWIDTH, SCREENHEIGHT, "Falling Sand");

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);


                if (pixelCount < 1000)
                {
                    createPixel(Raylib.GetMouseX(),Raylib.GetMouseY(), pixelReferences, Map);
                    pixelCount++;
                }


                updateMapVisuals(pixelReferences);
                Raylib.DrawText(Raylib.GetFPS().ToString(),5,5,10,Color.RAYWHITE);
                Raylib.DrawText("Number of pixels: " + pixelReferences.Count.ToString(),5,15,10,Color.RAYWHITE);
                                
                Raylib.EndDrawing();

                updatePhysicsComponents(pixelReferences, Map);
            }

            Raylib.CloseWindow();
        }

        static void updateMapVisuals(Stack<Pixel> pixelReferences){
            for (int i = 0; i < pixelReferences.Count; i++)
            {
                Pixel pixelToDraw = pixelReferences.ElementAt(i);
                Raylib.DrawRectangle(pixelToDraw.position.x - PIXELWIDTH, pixelToDraw.position.y - PIXELHEIGHT, PIXELWIDTH,PIXELHEIGHT,pixelToDraw.graphics.colour);
            }
        }

        static void createPixel(int x, int y, Stack<Pixel> pixelReferences, int[,] map){
            if (map[x,y] != 1)
            {
                if((x > 0 || x < SCREENWIDTH-1) || (y > 0 || y < SCREENHEIGHT-1)){
                    Pixel newPixel = new Pixel(new PositionComponent(x,y),new GraphicsComponent(Color.BEIGE),new PhysicsComponent(PhysicsTypes.FALL));
                    pixelReferences.Push(newPixel);
                }

            }
        }


        static void updatePhysicsComponents(Stack<Pixel> pixels, int[,] map){
            
            for (int i = 0; i < pixels.Count; i++)
            {
                if (pixels.ElementAt(i).physics.type == PhysicsTypes.FALL)
                {
                    if ((pixels.ElementAt(i).position.y < GRIDHEIGHT-1)) //Bound to ground
                    {
                        if(map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y+1] == 0) { //Directly Down
                        
                            if (pixels.ElementAt(i).position.y -1 >= 0)
                            {
                                map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 0; //Previous Position is now open
                            }
                        
                            pixels.ElementAt(i).position.y += 1;

                            map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 1; //New Position is occupied

                        }
                        else if ((map[pixels.ElementAt(i).position.x-1, pixels.ElementAt(i).position.y+1] == 0) && (pixels.ElementAt(i).position.x-1 > 0)) //Down-Left
                        {
                            map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 0; //Previous Position is now open

                            pixels.ElementAt(i).position.y += 1;
                            pixels.ElementAt(i).position.x -= 1;

                            map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 1; //New Position is occupied
                        }
                        else if ((map[pixels.ElementAt(i).position.x+1, pixels.ElementAt(i).position.y+1] == 0) && (pixels.ElementAt(i).position.x-1 < SCREENWIDTH-1)) //Down-Right
                        {
                            map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 0; //Previous Position is now open

                            pixels.ElementAt(i).position.y += 1;
                            pixels.ElementAt(i).position.x += 1;

                            map[pixels.ElementAt(i).position.x, pixels.ElementAt(i).position.y] = 1; //New Position is occupied
                        }
                    }

                }
            }
        }

    }

    class PositionComponent{
        public int x;
        public int y;

        public PositionComponent(int _x, int _y){
            x = _x;
            y = _y;
        }

    }

    class GraphicsComponent{
        public Color colour;

        public GraphicsComponent(Color _color){
            colour = _color;
        }
    }

    enum PhysicsTypes{
        FALL,
        FILL,
        RISE,
        STATIC,
    }

    class PhysicsComponent{
        public PhysicsTypes type;

        public PhysicsComponent(PhysicsTypes _type){
            type = _type;
        }
    }


    class Pixel{
        public PositionComponent position;
        public GraphicsComponent graphics;
        public PhysicsComponent physics;

        public Pixel(PositionComponent _position, GraphicsComponent _graphics, PhysicsComponent _physics){
            position = _position;
            graphics = _graphics;
            physics = _physics;
        }

    }

}

