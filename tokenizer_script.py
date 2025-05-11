from transformers import AutoTokenizer
import sys

tokenizer = AutoTokenizer.from_pretrained(r"D:\C#\GNAggregator\GNAggregator")

# Получаем путь к файлу
file_path = sys.argv[1]

# Читаем содержимое файла
with open(file_path, "r", encoding="utf-8") as f:
    text = f.read()

tokens = tokenizer(text, return_tensors="pt")["input_ids"].tolist()[0]
print(" ".join(map(str, tokens)))

sys.stdout.flush()
sys.exit(0)
