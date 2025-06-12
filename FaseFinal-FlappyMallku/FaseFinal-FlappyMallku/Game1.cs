using System;
using System.Collections.Generic; // <-- Necesario para List<>
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Data.SqlClient;



namespace Flappymallku
{
    public enum EstadoJuego
    {
        Start,
        Jugando,
        Score
    }

    public class Game1 : Game
    {
        // VARIABLES PRIVADAS
        private string nombreJugador = "";
        private List<ScoreRecord> listaScores = new List<ScoreRecord>();
        private DatabaseManager dbManager = new DatabaseManager();

        private Texture2D nicknameButtonTexture;
        private Rectangle nicknameButtonRect;
        private Texture2D exitButtonTexture;
        private Rectangle exitButtonRect;
        private Texture2D replayButtonTexture, menuButtonTexture;
        private Rectangle replayButtonRect, menuButtonRect;
        private Texture2D gameOverTexture;
        private bool mostrarGameOver = false;
        private double tiempoGameOver = 0;
        private const double duracionGameOver = 2.0; // segundos congelado
        private Texture2D texturaCorazon;
        private int vidas = 3;
        private Texture2D pointsTableTexture;
        private SpriteFont fuentePuntaje;
        private Texture2D FondoScore;
        private Rectangle scoreButtonRect;
        private Texture2D scoreButtonTexture;
        private Texture2D playButtonTexture;
        private Rectangle playButtonRect;
        private Puntaje puntaje;
        private Texture2D FondoStart; // Fondo para la pantalla de inicio
        private Texture2D FondosJugando; // Fondo principal para Jugando
        private Texture2D mallkuTexture; // Textura del pájaro
        private Fondos Fondos; // Instancia del fondo
        private Mallku mallku; // Instancia del pájaro
        private Texture2D pixel; // Textura para dibujar rectángulos
        private GraphicsDeviceManager graphics; // Configuración de la ventana y dispositivo gráfico
        private Random random = new Random(); // Para usar random en cualquier cosa
        private SpriteBatch spriteBatch; // Dibuja sprites
        private KeyboardState tecladobefore; // Permite usar el teclado
        private Tuberias[] tuberias; // Arreglo de tuberías
        private Texture2D tuberiaLowTexture; // Textura de la tubería
        private float velocidad = 5f; // Velocidad de movimiento de tuberías
        private EstadoJuego estadoActual = EstadoJuego.Start; // Comenzamos en la parte del inicio  
        private MouseState mouseBefore;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            tecladobefore = Keyboard.GetState();
        }

        private void reiniciarjuego()
        {
            // Reinicia la posición del mallku
            mallku.posicion = new Vector2(50, 100);
            mallku.ResetearVelocidad();

            // Reinicia las tuberías a su posición inicial
            for (int i = 0; i < tuberias.Length; i++)
            {
                float x = graphics.PreferredBackBufferWidth + i * 300;
                tuberias[i] = new Tuberias(tuberiaLowTexture, new Vector2(x, 0), graphics.PreferredBackBufferHeight, 180, velocidad);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Server=DESKTOP-E5MPDNC;Database=FlappyMallkuDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"))
                {
                    conn.Open();
                    Console.WriteLine("✅ Conexión a la base de datos exitosa.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al conectar a la base de datos: " + ex.Message);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            FondoStart = Content.Load<Texture2D>("painicio"); // Cambia por el nombre real de tu xnb
            playButtonTexture = Content.Load<Texture2D>("playButton");
            scoreButtonTexture = Content.Load<Texture2D>("scoreButton");
            nicknameButtonTexture = Content.Load<Texture2D>("nicknameButton");
            int nicknameButtonWidth = nicknameButtonTexture.Width;
            int nicknameButtonHeight = nicknameButtonTexture.Height;
            int nicknameButtonX = (graphics.PreferredBackBufferWidth - nicknameButtonWidth) / 2;
            int nicknameButtonY = 80; // Ajusta este valor para que quede arriba del logo
            nicknameButtonRect = new Rectangle(nicknameButtonX, nicknameButtonY, nicknameButtonWidth, nicknameButtonHeight);

            int buttonWidth = playButtonTexture.Width;
            int buttonHeight = playButtonTexture.Height;
            int buttonX = (graphics.PreferredBackBufferWidth - buttonWidth) / 2;
            int buttonY = (int)(graphics.PreferredBackBufferHeight * 0.75f) - (buttonHeight / 2);
            playButtonRect = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);

            // Botón Score centrado y más arriba (por ejemplo, a 60% de la altura)
            int scoreButtonWidth = scoreButtonTexture.Width;
            int scoreButtonHeight = scoreButtonTexture.Height;
            int scoreButtonX = (graphics.PreferredBackBufferWidth - scoreButtonWidth) / 2;
            int scoreButtonY = (int)(graphics.PreferredBackBufferHeight * 0.90f) - (scoreButtonHeight / 2);
            scoreButtonRect = new Rectangle(scoreButtonX, scoreButtonY, scoreButtonWidth, scoreButtonHeight);

            FondoScore = Content.Load<Texture2D>("pascore");

            exitButtonTexture = Content.Load<Texture2D>("exitButton");
            int exitButtonWidth = exitButtonTexture.Width;
            int exitButtonHeight = exitButtonTexture.Height;
            int exitButtonX = graphics.PreferredBackBufferWidth - exitButtonWidth - 20; // 20 píxeles de margen derecho
            int exitButtonY = graphics.PreferredBackBufferHeight - exitButtonHeight - 20; // 20 píxeles de margen inferior
            exitButtonRect = new Rectangle(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);

            FondosJugando = Content.Load<Texture2D>("fondofinal"); // Usa el nombre real de tu archivo sin extensión
            mallkuTexture = Content.Load<Texture2D>("mallku");
            Fondos = new Fondos(FondosJugando, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            mallku = new Mallku(mallkuTexture, new Vector2(50, 100));
            mallku.Pantalla = graphics.PreferredBackBufferHeight;

            texturaCorazon = Content.Load<Texture2D>("Vidas");

            pointsTableTexture = Content.Load<Texture2D>("points");
            fuentePuntaje = Content.Load<SpriteFont>("FuentePuntaje");
            gameOverTexture = Content.Load<Texture2D>("gameOver");
            replayButtonTexture = Content.Load<Texture2D>("replayButton");
            menuButtonTexture = Content.Load<Texture2D>("menuButton");
            int btnY = graphics.PreferredBackBufferHeight / 2 + 60;
            int btnSpacing = 40;
            int btnWidth = replayButtonTexture.Width;
            int btnHeight = replayButtonTexture.Height;
            int totalWidth = btnWidth * 2 + btnSpacing;
            int startX = (graphics.PreferredBackBufferWidth - totalWidth) / 2;

            menuButtonRect = new Rectangle(startX, btnY, btnWidth, btnHeight);
            replayButtonRect = new Rectangle(startX + btnWidth + btnSpacing, btnY, btnWidth, btnHeight);

            tuberiaLowTexture = Content.Load<Texture2D>("tuberiaLow");
            tuberias = new Tuberias[3];

            puntaje = new Puntaje();

            for (int i = 0; i < tuberias.Length; i++)
            {
                float x = graphics.PreferredBackBufferWidth + i * 300;
                tuberias[i] = new Tuberias(tuberiaLowTexture, new Vector2(x, 0), graphics.PreferredBackBufferHeight, 180, velocidad);
            }

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState tecladoAfter = Keyboard.GetState();
            MouseState mouseActual = Mouse.GetState();

            if (mostrarGameOver)
            {
                tiempoGameOver += gameTime.ElapsedGameTime.TotalSeconds;
                if (tiempoGameOver >= duracionGameOver)
                {
                    if (mouseActual.LeftButton == ButtonState.Pressed && mouseBefore.LeftButton == ButtonState.Released)
                    {
                        if (menuButtonRect.Contains(mouseActual.Position))
                        {
                            mostrarGameOver = false;
                            estadoActual = EstadoJuego.Start;
                            vidas = 3;
                        }
                        else if (replayButtonRect.Contains(mouseActual.Position))
                        {
                            mostrarGameOver = false;
                            estadoActual = EstadoJuego.Jugando;
                            vidas = 3;
                            reiniciarjuego();
                        }
                    }
                    mouseBefore = mouseActual;
                }
                tecladobefore = tecladoAfter;
                return;
            }

            if (estadoActual == EstadoJuego.Start)
            {
                var keys = Keyboard.GetState().GetPressedKeys();
                foreach (var key in keys)
                {
                    if (tecladobefore.IsKeyUp(key))
                    {
                        if (key == Keys.Back && nombreJugador.Length > 0)
                        {
                            nombreJugador = nombreJugador.Substring(0, nombreJugador.Length - 1);
                        }
                        else if (nombreJugador.Length < 12 && key >= Keys.A && key <= Keys.Z)
                        {
                            bool shift = Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift);
                            char letra = (char)(key - Keys.A + (shift ? 'A' : 'a'));
                            nombreJugador += letra;
                        }
                        else if (nombreJugador.Length < 12 && key >= Keys.D0 && key <= Keys.D9)
                        {
                            char numero = (char)('0' + (key - Keys.D0));
                            nombreJugador += numero;
                        }
                        // Puedes agregar más teclas si quieres permitir espacios, etc.
                    }
                }

                if (mouseActual.LeftButton == ButtonState.Pressed && playButtonRect.Contains(mouseActual.Position))
                {
                    estadoActual = EstadoJuego.Jugando;
                    reiniciarjuego();
                }
                else if (mouseActual.LeftButton == ButtonState.Pressed && scoreButtonRect.Contains(mouseActual.Position))
                {
                    try
                    {
                        listaScores = dbManager.LeerScores();
                        estadoActual = EstadoJuego.Score;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al leer los puntajes: " + ex.Message);                       
                        estadoActual = EstadoJuego.Start;
                    }
                }
            }

            if (estadoActual == EstadoJuego.Jugando)
            {
                // Haz que el fondo se desplace
                Fondos.Update(velocidad * 0.3f); // El fondo se moverá más lento que las tuberías

                if (tecladoAfter.IsKeyDown(Keys.Space) && tecladobefore.IsKeyUp(Keys.Space))
                {
                    mallku.Salto();
                }

                mallku.Update();

                foreach (var tubo in tuberias)
                {
                    tubo.Update();

                    if (mallku.posicion.X > tubo.posicion.X + tubo.ancho && !tubo.Contabilizada)
                    {
                        tubo.Contabilizada = true;
                        puntaje.Incrementar();
                    }
                }

                for (int i = 0; i < tuberias.Length; i++)
                {
                    if (mallku.GetBounds().Intersects(tuberias[i].GetBoundsSuperior()) ||
                        mallku.GetBounds().Intersects(tuberias[i].GetBoundsInferior()))
                    {
                        vidas--;
                        if (vidas <= 0)
                        {
                            dbManager.GuardarScore(nombreJugador, puntaje.Valor);
                            mostrarGameOver = true;
                            tiempoGameOver = 0;

                            puntaje.Reiniciar();
                        }
                        else
                        {
                            reiniciarjuego();
                        }
                        break;
                    }
                }
            }

            if (estadoActual == EstadoJuego.Score)
            {
                if (mouseActual.LeftButton == ButtonState.Pressed && mouseBefore.LeftButton == ButtonState.Released)
                {
                    if (exitButtonRect.Contains(mouseActual.Position))
                    {
                        estadoActual = EstadoJuego.Start;
                    }
                }
            }

            mouseBefore = mouseActual;
            tecladobefore = tecladoAfter;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (estadoActual)
            {
                case EstadoJuego.Start:
                    spriteBatch.Draw(FondoStart, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(nicknameButtonTexture, nicknameButtonRect, Color.White);

                    // Dibuja el nombre ingresado (con cursor parpadeante)
                    string mostrarNombre = nombreJugador;
                    Vector2 sizeNombre = fuentePuntaje.MeasureString(mostrarNombre);
                    Vector2 posNombre = new Vector2(
                             nicknameButtonRect.X + 160, // pequeño margen desde la izquierda del botón
                             nicknameButtonRect.Y + (nicknameButtonRect.Height - fuentePuntaje.MeasureString(nombreJugador).Y) / 2 // centrado vertical
                    );
                    spriteBatch.DrawString(fuentePuntaje, nombreJugador, posNombre, Color.White);
                    spriteBatch.DrawString(fuentePuntaje, mostrarNombre, posNombre, Color.Black);
                    if (DateTime.Now.Millisecond % 1000 < 500)
                    {
                        Vector2 posCursor = new Vector2(posNombre.X + sizeNombre.X + 5, posNombre.Y - 15);
                        spriteBatch.DrawString(fuentePuntaje, "|", posCursor, Color.White);
                    }
                    {
                        spriteBatch.Draw(pixel, new Rectangle((int)(posNombre.X + sizeNombre.X), (int)posNombre.Y, 2, (int)sizeNombre.Y), Color.Black);
                    }

                    spriteBatch.Draw(playButtonTexture, playButtonRect, Color.White);
                    spriteBatch.Draw(scoreButtonTexture, scoreButtonRect, Color.White);
                    break;

                case EstadoJuego.Jugando:
                    Fondos.Draw(spriteBatch);
                    mallku.Draw(spriteBatch);
                    foreach (var tubo in tuberias)
                        tubo.Draw(spriteBatch);

                    int tableWidth = pointsTableTexture.Width;
                    int tableHeight = pointsTableTexture.Height;
                    int margin = 20;
                    int tableX = graphics.PreferredBackBufferWidth - tableWidth - margin;
                    int tableY = margin;
                    spriteBatch.Draw(pointsTableTexture, new Rectangle(tableX, tableY, tableWidth, tableHeight), Color.White);
                    string textoScore = puntaje.Valor.ToString();

                    Vector2 size = fuentePuntaje.MeasureString(textoScore);
                    float scoreX = tableX + tableWidth - size.X - 25;
                    float scoreY = tableY + (tableHeight - size.Y) / 2;
                    spriteBatch.DrawString(fuentePuntaje, textoScore, new Vector2(scoreX, scoreY), Color.Black);

                    for (int j = 0; j < vidas; j++)
                    {
                        int corazonX = 20 + j * (texturaCorazon.Width + 10);
                        int corazonY = 20;
                        spriteBatch.Draw(texturaCorazon, new Rectangle(corazonX, corazonY, 32, 32), Color.White);
                    }

                    if (mostrarGameOver)
                    {
                        int goWidth = gameOverTexture.Width;
                        int goX = (graphics.PreferredBackBufferWidth - goWidth) / 2;
                        int goY = graphics.PreferredBackBufferHeight / 6;
                        spriteBatch.Draw(gameOverTexture, new Rectangle(goX, goY, goWidth, gameOverTexture.Height), Color.White);

                        spriteBatch.Draw(menuButtonTexture, menuButtonRect, Color.White);
                        spriteBatch.Draw(replayButtonTexture, replayButtonRect, Color.White);
                    }
                    break;

                case EstadoJuego.Score:
                    spriteBatch.Draw(FondoScore, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);


                    spriteBatch.Draw(exitButtonTexture, exitButtonRect, Color.White);

                    int yInicio = 100;
                    int espacioY = 30;
                    int xRanking = 100;
                    int xNombre = 150;
                    int xPuntaje = 600;

                    for (int i = 0; i < listaScores.Count && i < 10; i++)
                    {
                        var s = listaScores[i];
                        string nombre = s.Nickname.ToUpper();
                        string puntajeTexto = $"{s.Puntaje} pts";
                        Color colorTexto = 1 == 0 ? Color.Gold : Color.Lavender;
                        Color colorPuntaje = 1 == 0 ? Color.Gold : Color.LightCyan;
                        float posY = yInicio + i * espacioY;
                        spriteBatch.DrawString(fuentePuntaje, $"{i + 1}.", new Vector2(xRanking, posY), colorTexto);
                        spriteBatch.DrawString(fuentePuntaje, nombre, new Vector2(xNombre, posY), colorTexto);
                        Vector2 sizePuntaje = fuentePuntaje.MeasureString(puntajeTexto);
                        Vector2 posPuntaje = new Vector2(xPuntaje - sizePuntaje.X, posY);
                        spriteBatch.DrawString(fuentePuntaje, puntajeTexto, posPuntaje, colorPuntaje);

                    }
                    break;


            }
             spriteBatch.End();
        }
    }
}