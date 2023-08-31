from TikTokLive import TikTokLiveClient
from TikTokLive.types.events import CommentEvent, ConnectEvent, GiftEvent, LikeEvent, FollowEvent, ShareEvent, JoinEvent, DisconnectEvent, LiveEndEvent, ViewerUpdateEvent, UnknownEvent
import asyncio
import asyncTikTokScraper
from traceback import print_exc
from collections import deque
import socketApi
import time
import sys

pfpDownloadingQueue = []
pfpDownloaded = []
pfpDownloadFinished = []
# https://www.tiktok.com/@zh.xai
streamerName = "zh.xai" # goofee_69 # reddit_officially # meastkill # __domix__ # zh.xai (always check)
loop = asyncio.new_event_loop()
asyncio.set_event_loop(loop)

lastInteractionTime = time.monotonic() # to check if script is still getting data else restarting



# asyncio.new_event_loop().set_debug(True)

    
client: TikTokLiveClient = TikTokLiveClient(unique_id=f"@{streamerName}", loop=loop)

events = deque()


async def downloadWrapper(username: str, url: str):
        if username not in pfpDownloaded:
            pfpDownloaded.append(username)
            downloading = True
            pfpDownloadingQueue.append(username)

            while(downloading):
                if pfpDownloadingQueue[0] == username:

                    await (asyncTikTokScraper.downloadProfilePicture(url, username))                   
                    downloading = False
                    pfpDownloadingQueue.pop(0)
                    pfpDownloadFinished.append(username)
                    
                else:

                    await asyncio.sleep(0.5)

lastViewerAmount = 0

async def getViewerCount():
    while(True):
        print("Current viewer count: ", client.viewer_count)
        print("Currently connected: ", client.connected)

        lastInteractionDifference = time.monotonic() - lastInteractionTime
        print(lastInteractionDifference)

        if lastInteractionDifference > 60*30 and client.connected:
            print("last interaction was more than 3 seconds ago...\nRestarting script in 60 seconds")
            client.stop()
            print("stopped client")
            # print("Restarted script")
            # await asyncio.sleep(60)
            # await client.start()
        else:
            await asyncio.sleep(30)

@client.on("viewer_update")
async def on_connect(event: ViewerUpdateEvent):
   # print("stream has so many viewers: ", connectObject)
    global lastInteractionTime
    lastInteractionTime = time.monotonic()


@client.on("disconnect")
async def on_disconnect(event: DisconnectEvent):
    print("Disconnected, reconnecting in 2 minutes")
    await asyncio.sleep(120)
    await client.start()



@client.on("live_end")
async def on_connect(event: LiveEndEvent):
    print(f"Livestream ended :(")
    quit()


@client.on("connect")
async def on_connect(event: ConnectEvent):
        print("Connected to Room ID:", client.room_id)
        global lastInteractionTime
        lastInteractionTime = time.monotonic()

@client.on("comment")
async def on_comment(event: CommentEvent):
    print(f"{event.user.unique_id} -> {event.comment}")

    events.append(dict(user=event.user.unique_id, event="comment", comment=event.comment))
    await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
    global lastInteractionTime
    lastInteractionTime = time.monotonic()        

@client.on("gift")
async def on_gift(event: GiftEvent):
        # Streakable gift & streak is over
        if event.gift.streakable and not event.gift.streaking:
            print(f"{event.user.unique_id} sent {event.gift.count}x \"{event.gift.info.name}\"")

        # Non-streakable gift
        elif not event.gift.streakable:
            print(f"{event.user.unique_id} sent \"{event.gift.info.name}\"")

        events.append(dict( user=event.user.unique_id, event="gift", gift=event.gift.info.name, count=event.gift.count))
        await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
        global lastInteractionTime
        lastInteractionTime = time.monotonic()

@client.on("like")
async def on_like(event: LikeEvent):
        print(f"@{event.user.unique_id} liked the stream {event.likes} times!")
        for queueEvent in events:

              if queueEvent["user"] == event.user.unique_id and queueEvent["event"] == "like":
                    queueEvent["count"] += event.likes
                    return 
        events.append(dict( user=event.user.unique_id, event="like", count=event.likes))
        await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
        global lastInteractionTime
        lastInteractionTime = time.monotonic()

@client.on("follow")
async def on_follow(event: FollowEvent):
        print(f"@{event.user.unique_id} followed the streamer!")
        events.append(dict(user=event.user.unique_id, event="follow"))
        await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
        global lastInteractionTime
        lastInteractionTime = time.monotonic()

@client.on("share")
async def on_share(event: ShareEvent):
        print(f"@{event.user.unique_id} shared the stream!")
        events.append(dict(user=event.user.unique_id, event="share"))
        await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
        global lastInteractionTime
        lastInteractionTime = time.monotonic()
@client.on("error")
async def on_connect(error: Exception):
    # Handle the error
        print(f"An error ocurred in live api: {error}")
        print_exc()



@client.on("join")
async def on_join(event: JoinEvent):
    print(f"@{event.user.unique_id} joined the stream!")
    events.append(dict(user=event.user.unique_id, event="join"))
    await downloadWrapper(event.user.unique_id, event.user.avatar.urls[1])
    global lastInteractionTime
    lastInteractionTime = time.monotonic()


@client.on("unknown")
async def on_connect(event: UnknownEvent):
    print("Recieved unknown event: ", event)
    print(f"Event Type: {event.type}")
    print(f"Event Base64: {event.base64}")

async def stdoutFlusher():
    while True:
        sys.stdout.flush()
        await asyncio.sleep(5)

if __name__ == '__main__':

    # loop.run_until_complete(asyncTikTokScraper.init())
    loop.create_task(client.start())
    #loop.create_task(getViewerCount())
    loop.create_task(socketApi.start_local_server(events=events, downloadedPfp=pfpDownloadFinished))
    loop.create_task(stdoutFlusher())

    loop.run_forever()