import asyncio
import os
import time
from playwright.async_api import async_playwright
import aiohttp
url = "https://videodownloader.so/download/pfp/tiktok?v="

browser = None
page = None

async def init():


  global browser
  global page
  playwright = await async_playwright().start()
  chromium = playwright.chromium
  browser = await chromium.launch(headless=False)
  page = await browser.new_page()
  cwd = os.getcwd()
  if not os.path.exists(os.path.abspath(os.path.join(cwd, os.pardir))):
    os.mkdir("Resources")
  print("event loop init: ", id(asyncio.get_event_loop()))
  

async def downloadProfilePicture(url: str, username: str):  
    imgData = None
    print("started downloading: ", username)
    async with aiohttp.ClientSession() as session:
        async with session.get(url) as response:
            imgData = await response.read()    
    
    cwd = os.getcwd()
    parent_directory = os.path.dirname(cwd)
    print(parent_directory)
    with open(os.path.join(os.path.join(parent_directory, "Resources"), f"{username}.png"), "wb") as f:
        f.write(imgData)
    print("finished downloading: ", username)




async def getProfilePicture(username: str, loop):
  asyncio.set_event_loop(loop)
  print("event loop getpfp: ", id(asyncio.get_event_loop()))
  try:
    await page.goto(url+username)
  except Exception as e:
    print("exception occured during goto pfp download: ", e)
  try:
    imgContainer = await page.wait_for_selector(".thumbnail", state="visible")
  except Exception as e:
      print("exception occured during imgContainer pfp download: ", e)
  try:
    img = await imgContainer.wait_for_selector("img", state="visible")
  except Exception as e:
      print("exception occured during img pfp download: ", e)
  try:
    await img.screenshot(path=os.path.join("profilePics", f"{username}.png"))
  except Exception as e:
      print("exception occured during screenshot pfp download: ", e)


if __name__ == "__main__":

  
  loop = asyncio.new_event_loop()
  asyncio.set_event_loop(loop)
  async def main():
    await init()
    start = time.time()
    await getProfilePicture("ziemlichwild")
    await getProfilePicture("groovygoofymonster")
    print(time.time()-start)

  loop.run_until_complete(main()) 
