using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Fondos
{
    private Texture2D textura;
    private Rectangle area;
    private float posicionX;
    private int anchoPantalla;

    public Fondos(Texture2D textura, int anchoPantalla, int pantallaAltura)
    {
        this.textura = textura;
        this.anchoPantalla = anchoPantalla;
        area = new Rectangle(0, 0, anchoPantalla, pantallaAltura);
        posicionX = 0;
    }

    // Llama a este método en Update, pasando la velocidad deseada (por ejemplo, 2f)
    public void Update(float velocidad)
    {
        posicionX += velocidad;

        // Si llega al final, vuelve al principio (scroll infinito)
        if (posicionX > textura.Width - anchoPantalla)
            posicionX = 0;
        if (posicionX < 0)
            posicionX = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle origen = new Rectangle((int)posicionX, 0, area.Width, area.Height);
        spriteBatch.Draw(textura, area, origen, Color.White);

        // Si estamos cerca del final, dibuja el inicio de la textura a la derecha para un scroll suave
        if (posicionX > textura.Width - anchoPantalla)
        {
            int sobrante = (int)posicionX - (textura.Width - anchoPantalla);
            Rectangle origen2 = new Rectangle(0, 0, sobrante, area.Height);
            Rectangle destino2 = new Rectangle(area.Width - sobrante, 0, sobrante, area.Height);
            spriteBatch.Draw(textura, destino2, origen2, Color.White);
        }
    }

    // Si quieres reiniciar el fondo al principio
    public void Reset()
    {
        posicionX = 0;
    }
}