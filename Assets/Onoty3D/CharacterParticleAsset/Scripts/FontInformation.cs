using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Onoty3D.CharacterParticleAsset.Scripts
{
    [CreateAssetMenu(fileName = "fontInformation", menuName = "Onoty3D/Character Particle/Font Information")]
    public class FontInformation : ScriptableObject
    {
        #region CharacterInfo

        public class CharacterDetail
        {
            /// <summary>
            /// テクスチャ上の文字の矩形情報
            /// </summary>
            public Vector4 Area { get; set; }

            /// <summary>
            /// 矩形内の文字領域の情報
            /// </summary>
            public Vector4 Region { get; set; }

            public CharacterDetail(XmlAttributeCollection attributes, float baseSize, float textureWidth, float textureHeight)
            {
                //テクスチャ上の文字の矩形情報　テクスチャの大きさを1としたときの割合
                var area = new Vector4
                    (
                        float.Parse(attributes.GetNamedItem("x").Value) / textureWidth, //始点x
                        float.Parse(attributes.GetNamedItem("y").Value) / textureHeight, //始点y
                        float.Parse(attributes.GetNamedItem("width").Value) / textureWidth, //幅
                        float.Parse(attributes.GetNamedItem("height").Value) / textureHeight //高さ
                    );
                area.y = 1 - (area.y + area.w);
                this.Area = area;

                //矩形内の文字領域の情報　矩形の大きさを1とした時の割合
                var region = new Vector4
                    (
                        float.Parse(attributes.GetNamedItem("width").Value) / baseSize, //幅
                        float.Parse(attributes.GetNamedItem("height").Value) / baseSize, //高さ
                                                                                         //float.Parse(attributes.GetNamedItem("xoffset").Value) / baseSize, //xoffset(センタリングするので計算して求める)
                        0,
                        float.Parse(attributes.GetNamedItem("yoffset").Value) / baseSize //yoffset
                    );
                region.z = (1 - region.x) / 2;
                region.w = 1 - (region.y + region.w);
                this.Region = region;
                //Debug.Log(this.Area);
                //Debug.Log(this.Region);
            }
        }

        #endregion CharacterInfo

        #region Property

        [Tooltip("configuration file(xml)")]
        [SerializeField]
        private TextAsset _fontXml;

        public TextAsset FontXml
        {
            get
            {
                return this._fontXml;
            }

            set
            {
                this._fontXml = value;
                this.SetCharacterDic();
            }
        }

        #endregion Property

        #region Field

        private Dictionary<char, CharacterDetail> _characterDic = new Dictionary<char, CharacterDetail>();

        #endregion Field

        #region Event Function

        private void OnEnable()
        {
            this.SetCharacterDic();
        }

        private void OnValidate()
        {
            this.SetCharacterDic();
        }

        #endregion Event Function

        #region Public Method

        [Obsolete("use FontXml property")]
        public void SetXml(TextAsset xml)
        {
            this._fontXml = xml;
            this.SetCharacterDic();
        }

        public bool TryGetCharacterDetail(char character, out CharacterDetail detail)
        {
            return this._characterDic.TryGetValue(character, out detail);
        }

        #endregion Public Method

        #region Private Method

        private void SetCharacterDic()
        {
            this._characterDic.Clear();

            if (this._fontXml == null)
            {
                return;
            }

            //xmlの各node,attributeは存在することを前提で処理書きます

            var document = new XmlDocument();

            document.LoadXml(_fontXml.text);
            var common = document.GetElementsByTagName("common");
            var attributes = (common.Item(0) as XmlNode).Attributes;

            var baseSize = float.Parse(attributes.GetNamedItem("lineHeight").Value);

            var textureWidth = float.Parse(attributes.GetNamedItem("scaleW").Value);

            var textureHeight = float.Parse(attributes.GetNamedItem("scaleH").Value);

            var characters = document.GetElementsByTagName("char");
            int id;
            foreach (XmlNode character in characters)
            {
                attributes = character.Attributes;
                id = int.Parse(attributes.GetNamedItem("id").Value);
                //Debug.Log((char)id);
                this._characterDic.Add((char)id, new CharacterDetail(attributes, baseSize, textureWidth, textureHeight));
            }
        }

        #endregion Private Method
    }
}