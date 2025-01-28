if CLIENT then
    -- Make sure the database path points to the correct file
    -- This refers to the default 'cl.db' in the Garry's Mod folder
    sql.Query("ATTACH DATABASE 'garrysmod/cl.db' AS client_db")

    -- Check if the table exists before attempting to delete data
    if sql.TableExists("client_db.damage_data") then
        print("SQL Table Found: Deleting Old Data")
        -- Clear the table to prevent the file from getting too large
        sql.Query("DELETE FROM client_db.damage_data")
    else
        print("SQL Table Not Found: Creating Table")
        -- Create the table since it doesn't exist
        sql.Query("CREATE TABLE client_db.damage_data (id INTEGER PRIMARY KEY AUTOINCREMENT, damage_type TEXT, direction TEXT)")
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

            -- Insert the data into the database
            local query = string.format(
                "INSERT INTO damage_data (damage_type, direction) VALUES (%s, %s)",
                sql.SQLStr(damagetype),
                sql.SQLStr(direction)
            )

            local result = sql.Query(query)
            if result == false then
                print("SQL Error: " .. sql.LastError())
            end
        end
    end)
end
