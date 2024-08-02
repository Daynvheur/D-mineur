#!/bin/sh
echo -ne '\033c\033]0;D-mineur\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/D-mineur.x86_64" "$@"
