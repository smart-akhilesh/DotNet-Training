
from openai import OpenAI

client = OpenAI(api_key="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")

def call_llm(prompt):
    response = client.chat.completions.create(
        model="gpt-4.1-mini",
        messages=[{"role": "user", "content": prompt}]
    )
    return response.choices[0].message.content
