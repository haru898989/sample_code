"""
Python UDP送信の基本テンプレート
"""
import socket
import json
import time

# 送信先のIPとポート番号（Unity側と必ず合わせる）
UDP_IP = "127.0.0.1" # ローカルホスト（同じPC内での通信）
UDP_PORT = 5020

# UDP通信用の「ソケット（通信の窓口）」を作成
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

print("UDP送信を開始します...")

try:
    while True:
        # 1. 送りたいデータを辞書型で作る
        data = {
            "message": "Hello from Python!",
            "value": 12.5
        }

        # 2. 辞書データをJSON文字列に変換
        json_str = json.dumps(data)

        # 3. 文字列をバイトデータ（機械が読める形）に変換して送信
        sock.sendto(json_str.encode('utf-8'), (UDP_IP, UDP_PORT))
        
        print(f"送信: {json_str}")

        # 1秒ごとに送信する（実際はループの速度に合わせて調整）
        time.sleep(1) 

except KeyboardInterrupt:
    # Ctrl+C で安全に終了するための処理
    print("終了します")
    sock.close()