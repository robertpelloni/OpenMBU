//-----------------------------------------------------------------------------
// Super Monkey Ball: Party Game Base Helpers
//-----------------------------------------------------------------------------

function PartyGame::initClientScore(%client, %message)
{
   if (isObject(%client))
   {
      %client.score = 0;
      messageClient(%client, 'MsgSystem', %message);
   }
}

function PartyGame::endGameUI(%client, %message)
{
   if (isObject(%client))
   {
      bottomPrint(%client, %message, 5, 2);
   }
}
