from PIL import Image
import os

def convert_to_black_and_white(image_path, output_directory):
    img = Image.open(image_path)
    bw_img = img.convert("L")
    filename, ext = os.path.splitext(os.path.basename(image_path))
    new_filename = filename + "-blackwhite" + ext
    output_path = os.path.join(output_directory, new_filename)
    bw_img.save(output_path)
    print(f"Converted {filename} to black and white: {new_filename}")

def main():
    directory = os.path.dirname(os.path.abspath(__file__))
    output_directory = os.path.join(directory, "black_and_white")
    os.makedirs(output_directory, exist_ok=True)

    for filename in os.listdir(directory):
        if filename.lower().endswith(('.png', '.jpg', '.jpeg', '.gif', '.bmp')):
            image_path = os.path.join(directory, filename)
            convert_to_black_and_white(image_path, output_directory)

if __name__ == "__main__":
    main()