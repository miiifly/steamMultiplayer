using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Multiplayer.Lobby
{
    public class PlayerInfoDisplay : NetworkBehaviour
    {
        [SyncVar(hook = nameof(HandleSteamIdUpdated))]
        private ulong steamId;

        [SerializeField]
        private RawImage _profileImage = null;
        [SerializeField]
        private TMP_Text _displayNameText = null;

        protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

        #region Server

        public void SetSteamId(ulong steamId)
        {
            this.steamId = steamId;
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
        }

        private void HandleSteamIdUpdated(ulong odlSteamid, ulong newStemId)
        {
            var cSteamId = new CSteamID(newStemId);

            _displayNameText.text = SteamFriends.GetFriendPersonaName(cSteamId);

            int imageId = SteamFriends.GetLargeFriendAvatar(cSteamId);

            if(imageId == -1)
            {
                return;
            }

            _profileImage.texture = GetSteamImageAsTexture(imageId);
        }

        private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
        {
            if(callback.m_steamID.m_SteamID == steamId)
            {
                return;
            }

            _profileImage.texture = GetSteamImageAsTexture(callback.m_iImage);
        }

        private Texture2D GetSteamImageAsTexture(int imageId)
        {
            Texture2D texture = null;

            bool isValid = SteamUtils.GetImageSize(imageId, out uint width, out uint height);

            if(isValid)
            {
                byte[] image = new byte[width * height*4];

                isValid = SteamUtils.GetImageRGBA(imageId,image,(int)(width*height*4));
                if(isValid )
                {
                    texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false,true);

                    texture.LoadRawTextureData(image);
                    texture.Apply();
                }
            }

            return texture;
        }
        #endregion
    }
}
