using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public partial class WangTileGeneratorSO : AbstractTileGeneratorSO
    {
        private List<WangTile> _visited;

        [Range(0, 1f)]
        public float selectionPercent = .5f;
        
        private void GeneratePerfectMaze(int width, int height)
        {
            _visited = new List<WangTile>();
            
            // Scelgo il punto di partenza (una tile, ad esempio (0, 0))
            // Se vogliamo, introduciamo un ingresso
            // Aggiungo la tile alla lista
            
            // Incomincio il ciclo
            // --> Prendi una tile a caso dalla lista
            // --> Controlla se ha vie di uscita (se ci sono zone limitrofe senza tile)
            // --> In caso positivo:
            //       --> Crea una connessione tra le due tile
            //       --> Aggiungi la nuova tile alla lista visitata
            // --> In caso negativo:
            //       --> Rimuovi la tile dalla lista
            // --> Quando la lista è vuota, il labirinto è finito
            
            
        }
    }
}
