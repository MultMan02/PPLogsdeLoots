#!/usr/bin/env python3
"""
Gerador de Log de Loot para o Trabalho de Programação Paralela
Formato do arquivo: ID_Item;Valor;Jogador;Timestamp
Uso: python gerar_log.py [numero_linhas] [arquivo_saida]
"""

import random
import sys
from datetime import datetime, timedelta

def gerar_log(num_linhas=10000000, nome_arquivo="loot_log.txt"):
    """
    Gera um arquivo de log no formato:
    ID_Item;Valor;Jogador;Timestamp
    
    Args:
        num_linhas: Quantidade de registros a gerar
        nome_arquivo: Nome do arquivo de saída
    """
    
    # Lista de itens possíveis (lore do jogo)
    itens = [
        "Espada_Flamejante", "Poção_de_Vida", "Elmo_Épico", "Moeda_de_Ouro",
        "Gema_Rara", "Armadura_Encantada", "Anel_da_Invisibilidade", "Arco_Longo",
        "Cajado_Arcano", "Escudo_Defensor", "Botas_da_Velocidade", "Amuleto_da_Sorte",
        "Adaga_Venenosa", "Manopla_do_Gigante", "Capa_da_Sombra"
    ]
    
    # Lista de jogadores (clãs em guerra)
    jogadores = [
        "Thorin_Escudo", "Legolas_Verde", "Gimli_Martelo", "Aragorn_Herói",
        "Gandalf_Branco", "Frodo_Bolseiro", "Arwen_Estrela", "Sauron_Sombrio",
        "Saruman_Branco", "Bilbo_Travesso", "Galadriel_Luz", "Elrond_Sábio"
    ]
    
    print(f"🚀 Gerando {num_linhas:,} registros...")
    print(f"📁 Arquivo destino: {nome_arquivo}")
    print("⏳ Isso pode levar alguns segundos...\n")
    
    data_inicial = datetime(2024, 1, 1)
    
    with open(nome_arquivo, 'w', encoding='utf-8') as arquivo:
        for i in range(1, num_linhas + 1):
            # Gera dados realistas
            item = random.choice(itens)
            # Valores entre 0.01 e 9999.99 com 2 casas decimais
            valor = round(random.uniform(0.01, 9999.99), 2)
            jogador = random.choice(jogadores)
            
            # Timestamp progressivo (simula eventos durante o jogo)
            timestamp = data_inicial + timedelta(seconds=random.randint(0, 31536000))
            timestamp_str = timestamp.strftime("%Y-%m-%d %H:%M:%S")
            
            # Escreve linha no formato: ID;VALOR;JOGADOR;TIMESTAMP
            arquivo.write(f"{item};{valor:.2f};{jogador};{timestamp_str}\n")
            
            # Progresso a cada 1 milhão de linhas
            if i % 1000000 == 0:
                print(f"✓ {i:,} registros gerados...")
    
    # Calcula tamanho do arquivo
    import os
    tamanho_bytes = os.path.getsize(nome_arquivo)
    tamanho_mb = tamanho_bytes / (1024 * 1024)
    
    print(f"\n✅ Arquivo gerado com sucesso!")
    print(f"📊 Total de registros: {num_linhas:,}")
    print(f"💾 Tamanho do arquivo: {tamanho_mb:.2f} MB")
    print(f"📝 Formato: ID_Item;Valor;Jogador;Timestamp")
    
    # Mostra amostra
    print("\n📋 Amostra das primeiras 5 linhas:")
    with open(nome_arquivo, 'r', encoding='utf-8') as f:
        for _ in range(5):
            print(f"   {f.readline().strip()}")
    
    return nome_arquivo

def main():
    # Parâmetros via linha de comando
    num_linhas = 10000000  # 10 milhões (padrão)
    nome_arquivo = "loot_log.txt"
    
    if len(sys.argv) > 1:
        try:
            num_linhas = int(sys.argv[1])
        except ValueError:
            print("❌ Erro: O primeiro argumento deve ser um número inteiro.")
            sys.exit(1)
    
    if len(sys.argv) > 2:
        nome_arquivo = sys.argv[2]
    
    # Gera o arquivo
    try:
        gerar_log(num_linhas, nome_arquivo)
    except Exception as e:
        print(f"❌ Erro ao gerar arquivo: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()