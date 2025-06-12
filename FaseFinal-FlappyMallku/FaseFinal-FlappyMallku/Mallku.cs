using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//         return new Rectangle((int)posicion.X, pantallaAltura - altoInferior, textura.Width, altoInferior);
public class Mallku
{
    private Texture2D texture;
    public void ResetearVelocidad()
    {
        velocidad = 0;
    }
    private float rotacion = 0f;
    private float velocidad;
    private float gravedad = 0.3f;
    private float distanciaSalto = -7f;
    public Vector2 posicion;
    public float Pantalla { get; set; }
    private readonly float escala = 0.5f;
    // Constructor que recibe la textura del pájaro y su posición inicial
    // y la altura de la pantalla para limitar el movimiento del pájaro
    public Mallku(Texture2D texture, Vector2 posicionInicial)
    {
        this.texture = texture;
        this.posicion = posicionInicial;
        this.velocidad = 0;

    }
    public void Update()
    {
        velocidad += gravedad;
        posicion.Y += velocidad;
        //Limmitación de que no se vaya hacia abajo o arriba de manera infinita
        if (posicion.Y + texture.Height * escala >= Pantalla)
        {
            posicion.Y = Pantalla - texture.Height * escala;
            velocidad = 0;
        }
        if (posicion.Y < 0)
        {
            posicion.Y = 0;
            velocidad = 0;
        }
        rotacion = MathHelper.Clamp(velocidad * 0.05f,-0.5f, 1f);
    }
    public void Salto()
    {
        velocidad = distanciaSalto;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, posicion, null, Color.White, rotacion, new Vector2(texture.Width / 2f, texture.Height / 2f), escala, SpriteEffects.None, 0f);
        

    }
    public Rectangle GetBounds()
    {
        int ancho = (int)(texture.Width * escala);
        int alto = (int)(texture.Height * escala);
        int margenHorizontal = 20; // Margen para evitar colisiones demasiado estrictas
        int margenVertical = 40; // Margen para evitar colisiones demasiado estrictas
        int reduccionAlto = 1;
        int x = (int)(posicion.X - ancho / 2f + margenHorizontal);
        int y = (int)(posicion.Y - alto / 2f + margenVertical + reduccionAlto / 2);
        return new Rectangle(x, y, ancho - 2 * margenHorizontal, alto - 2 * margenVertical);

    }
}