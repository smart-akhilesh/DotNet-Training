
from services.llm_service import call_llm
import json

def process_meeting(transcript):
    prompt = f"""
You are an AI meeting assistant.

Analyze the transcript and return JSON with:
- summary
- key_points
- tasks (task, owner, deadline, priority)
- risks
- decisions

Transcript:
{transcript}
"""

    response = call_llm(prompt)

    try:
        return json.loads(response)
    except:
        return {"raw_output": response}
