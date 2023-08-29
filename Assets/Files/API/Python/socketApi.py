import asyncio
import json
from functools import partial
from collections import deque
import os

HOST = "127.0.0.1"
PORT = 25001

async def handle_msg(reader: asyncio.StreamReader, writer: asyncio.StreamWriter, events: deque, downloadedPfp: list):
    try:
        data = None
        minLikes = 0
        while data != b"quit":
            try:
                data = await reader.read(1024)
            except:
                print("Connection error")
                return
            msg = data.decode().strip()
            
            if msg.split(" ")[0] == "setMinLikes":
                minLikes = int(msg.split(" ")[1])
                print(f"set min likes to {msg.split(' ')[1]}")

            elif msg == "endRound":
                events.clear()

            elif msg == "getEvent":
                addr, port = writer.get_extra_info("peername")

        
                foundValidEvent = False
                searchedEvents = deque()
                
                while not foundValidEvent:
                        if len(events) > 0:

                            curEvent = events.popleft()
                            if curEvent["user"] in downloadedPfp:
                                print(curEvent, type(curEvent))
                                if curEvent["event"] == "like":
                                    if curEvent["count"] >= minLikes:

                                        foundValidEvent = True
                                        response = json.dumps(curEvent)
                                    else:

                                        searchedEvents.appendleft(curEvent)
                        
                                else:
                                    
                                    foundValidEvent = True
                                    response = json.dumps(curEvent)
                            else:
                                #print(f"{curEvent['user']} pfp has not been downloaded: ",os.path.isfile(os.path.join('profilePics', f'{curEvent["user"]}.png')))
                                searchedEvents.appendleft(curEvent)
                        else:

                            foundValidEvent = True
                            response = "None"

                events.extendleft(list(searchedEvents))        
                

                
                writer.write(response.encode())
                await writer.drain()

        writer.close()
        await writer.wait_closed()
    except:
        print("resetting socket")


async def start_local_server(events: deque, downloadedPfp: list):

    partial_callback_func = partial(handle_msg, events=events, downloadedPfp=downloadedPfp)

    server = await asyncio.start_server(partial_callback_func, HOST, PORT)
    async with server:
        await server.serve_forever()


if __name__ == "__main__":
    loop = asyncio.new_event_loop()
    loop.run_until_complete(start_local_server("test"))