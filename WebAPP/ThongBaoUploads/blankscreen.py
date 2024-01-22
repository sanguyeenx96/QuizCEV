import pyautogui
import time
pyautogui.FAILSAFE = False

# Lấy kích thước màn hình
screen_width, screen_height = pyautogui.size()

# Tạo cửa sổ màn hình đen
pyautogui.click(0, 0)  # Bấm vào góc trái màn hình để đảm bảo cửa sổ không bị mất tiêu đề

# Chờ 5 giây
time.sleep(5)

# Đóng cửa sổ màn hình đen
pyautogui.hotkey("alt", "f4")  # Tắt bằng tổ hợp phím Alt + F4
