using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


public class Tuberias
{
    private Texture2D textura;
    public Vector2 posicion;
    private int espacioEntreTuberias = 150;
    private float velocidad = 2f;
    private int pantallaAltura;

    private int altoSuperior;
    private int altoInferior;
    public bool Contabilizada = false;
    public float ancho
    {
        get { return textura.Width; }
    }

    public Tuberias(Texture2D textura, Vector2 posicionInicial, int pantallaAltura, int espacioEntreTuberias = 150, float velocidad = 2f)
    {
        this.textura = textura;
        this.posicion = posicionInicial;
        this.pantallaAltura = pantallaAltura;
        this.espacioEntreTuberias = 150; // Espacio entre las tuberías
        this.velocidad = 2f; // Velocidad de las tuberías

        GenerarAlturasAleatorias();
    }

    public void Update()
    {
        posicion.X -= velocidad;

        if (posicion.X + textura.Width < 0)
        {
            posicion.X = 800;
            GenerarAlturasAleatorias();
            Contabilizada = false;
        }
    }

    public void GenerarAlturasAleatorias()
    {
        altoSuperior = Random.Shared.Next(100, pantallaAltura - espacioEntreTuberias - 100);
        altoInferior = pantallaAltura - altoSuperior - espacioEntreTuberias;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Superior (rotado)
        Rectangle origenSuperior = new Rectangle(0, 0, textura.Width, textura.Height);
        spriteBatch.Draw(textura, new Rectangle((int)posicion.X, 0, textura.Width, altoSuperior), origenSuperior, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

        // Inferior
        Rectangle origenInferior = new Rectangle(0, 0, textura.Width, textura.Height);
        spriteBatch.Draw(textura, new Rectangle((int)posicion.X, pantallaAltura - altoInferior, textura.Width, altoInferior), origenInferior, Color.White);
    }

    public Rectangle GetBoundsSuperior()
    {
        return new Rectangle((int)posicion.X, 0, textura.Width, altoSuperior);
    }

    public Rectangle GetBoundsInferior()
    {
        return new Rectangle((int)posicion.X, pantallaAltura - altoInferior, textura.Width, altoInferior);
    }
}
// Este código define la clase Tuberias que maneja las tuberías del juego Flappy Bird.
// La clase incluye métodos para actualizar la posición de las tuberías, dibujarlas en la pantalla,
