import os
import random
from moviepy.editor import *

def create_video_with_audio(image_folder, audio_path, video_path):
    # Get a list of image files in the folder
    image_files = [f for f in os.listdir(image_folder) if f.endswith(('.png', '.jpg', '.jpeg'))]
    
    if not image_files:
        raise ValueError("No image files found in the folder")

    # Randomly select an image
    image_path = os.path.join(image_folder, random.choice(image_files))
    
    # Load the image
    image_clip = ImageClip(image_path)
    
    # Load the audio
    audio_clip = AudioFileClip(audio_path)

    # Set the audio to the image
    video = image_clip.set_audio(audio_clip)

    # Set the duration of the video to match the audio
    video = video.set_duration(audio_clip.duration)

    # Set the frame rate for the video (since it's a static image, 1 fps is enough)
    video = video.set_fps(1)

    # Write the final video file to the specified path
    video.write_videofile(video_path, codec="libx264", audio_codec="aac")

# File paths
image_folder = r"D:\SwapWork\Text-to-Speech\Output\Images"
audio_path = r"D:\SwapWork\Text-to-Speech\Output\HobbiesandPersonalProjects\HobbiesandPersonalProjects.mp3"
video_path = r"D:\SwapWork\Text-to-Speech\Output\HobbiesandPersonalProjects\HobbiesandPersonalProjects.mp4"

# Create video with audio
create_video_with_audio(image_folder, audio_path, video_path)

