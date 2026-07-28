#!/bin/bash

OUTPUT_FILE="output.txt"
TARGET_DIR="${1:-.}"  # Use first argument as directory, default to current dir

> "$OUTPUT_FILE"  # Clear/create the output file

find "$TARGET_DIR" -type f | while read -r file; do
    echo "=== $file ===" >> "$OUTPUT_FILE"
    cat "$file" >> "$OUTPUT_FILE"
    echo "" >> "$OUTPUT_FILE"
done

echo "Done! Output written to $OUTPUT_FILE"