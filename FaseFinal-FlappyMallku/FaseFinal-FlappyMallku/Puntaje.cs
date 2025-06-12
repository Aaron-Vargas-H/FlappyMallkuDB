using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2=Microsoft.Xna.Framework.Vector2;
namespace Flappymallku
{
    public class Puntaje
    {
        private int score;
        public int Valor => score;
        public float escalaPuntaje;

        public void Reiniciar()
        {
            score = 0;
        }

        public void Incrementar()
        {
            score++;
        }

        // Si no usas este método, puedes eliminarlo
        public void Dibujar(SpriteBatch spriteBatch, SpriteFont fuente, int anchoPantalla)
        {
            // Implementa aquí si necesitas dibujar el puntaje
        }
    }
}