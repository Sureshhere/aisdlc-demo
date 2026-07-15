#!/usr/bin/env python3
import anthropic
import os
import sys

# Read the diff file
with open('pr_short.diff', 'r') as f:
    diff = f.read()

# Get API key from environment
api_key = os.getenv('ANTHROPIC_API_KEY')
if not api_key:
    print("Error: ANTHROPIC_API_KEY not set")
    sys.exit(1)

# Call Claude API
try:
    client = anthropic.Anthropic(api_key=api_key)
    message = client.messages.create(
        model="claude-haiku-4-5-20251001",
        max_tokens=1024,
        messages=[{
            "role": "user",
            "content": f"Review this PR diff for security issues, bugs, and code quality:\n\n{diff}\n\nProvide a concise review."
        }]
    )
    
    review = message.content[0].text
    
    # Write review to file
    with open('review.txt', 'w') as f:
        f.write(review)
    
    print(review)
    print("✓ Review generated successfully")
    
except Exception as e:
    print(f"Error: {e}", file=sys.stderr)
    sys.exit(1)
