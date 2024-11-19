Vi tilføjer brugere til databasen, som har granular access control placeret på sig.
Disse brugere bliver tilføjet med et sql script og skal gøres manuelt. 
Dette bliver vi nød til at gøre fordi det vil være en sikkerhedsrisiko at placerer dette i migrations da det ville kunne observeres i vores pipeline.
Sql scriptet, og initial login, kan findes under Google Drive: https://drive.google.com/drive/folders/1iVHGssY5298w93L-ENWl5bB0-NIpXQ-A