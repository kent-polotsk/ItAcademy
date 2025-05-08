from transformers import AutoTokenizer
import sys

tokenizer = AutoTokenizer.from_pretrained(r"D:\C#\GNAggregator\GNAggregator")
text = sys.argv[1]

tokens = tokenizer(text, return_tensors="pt")["input_ids"].tolist()[0]
print(" ".join(map(str, tokens)))

sys.stdout.flush() 
sys.exit(0)
