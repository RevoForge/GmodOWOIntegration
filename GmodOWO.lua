if CLIENT then
    -- Path to store the data file
    local filePath = "damage_data.json"
    print("OWO Mod Running")

    -- Function to write data to a JSON file
    local function WriteDataToJson(damageData)
        -- Convert the table to JSON format
        local jsonData = util.TableToJSON(damageData, true)

        -- Check if the file exists first
        if not file.Exists(filePath, "DATA") then
            -- Create the file by opening it in write mode
            local file = file.Open(filePath, "w", "DATA")
            if file then
                file:Write(jsonData)
                file:Close()
            else
                print("Error creating file")
            end
        else
            -- If the file exists, simply open and write to it
            local file = file.Open(filePath, "w", "DATA")  -- Open the file in write mode
            if file then
                file:Write(jsonData)
                file:Close()
            else
                print("Error opening file")
            end
        end
    end

    -- Create a table of damage types for the hook
    DmgTypeMap = {
        [DMG_GENERIC] = "DMG_GENERIC",
        [DMG_CRUSH] = "DMG_CRUSH",
        [DMG_BULLET] = "DMG_BULLET",
        [DMG_SLASH] = "DMG_SLASH",
        [DMG_BURN] = "DMG_BURN",
        [DMG_VEHICLE] = "DMG_VEHICLE",
        [DMG_FALL] = "DMG_FALL",
        [DMG_BLAST] = "DMG_BLAST",
        [DMG_CLUB] = "DMG_CLUB",
        [DMG_SHOCK] = "DMG_SHOCK",
        [DMG_SONIC] = "DMG_SONIC",
        [DMG_ENERGYBEAM] = "DMG_ENERGYBEAM",
        [DMG_PREVENT_PHYSICS_FORCE] = "DMG_PREVENT_PHYSICS_FORCE",
        [DMG_NEVERGIB] = "DMG_NEVERGIB",
        [DMG_ALWAYSGIB] = "DMG_ALWAYSGIB",
        [DMG_DROWN] = "DMG_DROWN",
        [DMG_PARALYZE] = "DMG_PARALYZE",
        [DMG_NERVEGAS] = "DMG_NERVEGAS",
        [DMG_POISON] = "DMG_POISON",
        [DMG_RADIATION] = "DMG_RADIATION",
        [DMG_DROWNRECOVER] = "DMG_DROWNRECOVER",
        [DMG_ACID] = "DMG_ACID",
        [DMG_SLOWBURN] = "DMG_SLOWBURN",
        [DMG_REMOVENORAGDOLL] = "DMG_REMOVENORAGDOLL",
        [DMG_PHYSGUN] = "DMG_PHYSGUN",
        [DMG_PLASMA] = "DMG_PLASMA",
        [DMG_AIRBOAT] = "DMG_AIRBOAT",
        [DMG_DISSOLVE] = "DMG_DISSOLVE",
        [DMG_BLAST_SURFACE] = "DMG_BLAST_SURFACE",
        [DMG_DIRECT] = "DMG_DIRECT",
        [DMG_BUCKSHOT] = "DMG_BUCKSHOT",
        [DMG_SNIPER] = "DMG_SNIPER",
        [DMG_MISSILEDEFENSE] = "DMG_MISSILEDEFENSE"
    }

    gameevent.Listen("player_hurt")
    hook.Add("player_hurt", "player_hurt_OWO", function(data)
        local player = LocalPlayer()
        print("Hurt Event Triggered")

        local id = data.userid
        if id == player:UserID() then
            local damagetype = "DMG_BULLET"
            local direction = "None"

            local attackerid = data.attacker
            if attackerid == 0 then
                damagetype = "DMG_FALL"
            else
                -- Needed for position calculation
                local attacker = Player(attackerid)

                local playerpos = player:EyePos()
                local attackerpos = attacker:EyePos()

                -- Calculate the relative direction of the damage origin
                direction = (attackerpos - playerpos):GetNormalized()

                -- Simplify the direction to front, back, left, right, etc.
                local playerForward = player:GetAimVector()
                local dotProduct = direction:Dot(playerForward)

                if dotProduct > 0.5 then
                    direction = "Front"
                elseif dotProduct < -0.5 then
                    direction = "Back"
                else
                    direction = (direction:Dot(player:GetRight()) > 0) and "Right" or "Left"
                end
            end

            -- Store the data as a table
            local damageData = {
                damage_type = damagetype,
                direction = direction
            }
            print("Hurt Event Sent")

            -- Write the data to the JSON file
            WriteDataToJson(damageData)
        end
    end)
end
