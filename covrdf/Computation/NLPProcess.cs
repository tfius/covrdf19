//using OpenNLP.Tools.Lang.English;
//using OpenNLP.Tools.PosTagger;
//using OpenNLP.Tools.SentenceDetect;
//using OpenNLP.Tools.Tokenize;
using covrdf.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace covrdf.Computation
{
    
    /*
    public class NLPProcess
    {
        EnglishMaximumEntropySentenceDetector sentenceDetector;
        EnglishMaximumEntropyTokenizer tokenizer;
        EnglishMaximumEntropyPosTagger posTagger;
        TreebankLinker coreferenceFinder;
        public NLPProcess(EnglishMaximumEntropySentenceDetector sentenceDetector,
                   EnglishMaximumEntropyTokenizer tokenizer,
                   EnglishMaximumEntropyPosTagger posTagger,
                   TreebankLinker coreferenceFinder)
        {
            this.sentenceDetector = sentenceDetector;
            this.tokenizer = tokenizer;
            this.posTagger = posTagger;
            this.coreferenceFinder = coreferenceFinder;
        }
        public ParagraphAnalysis AnalyseParagraph(string paragraphText)
        {
            var pa = new ParagraphAnalysis();
            pa.sentences = sentenceDetector.SentenceDetect(paragraphText);
            pa.coreference = coreferenceFinder.GetCoreferenceParse(pa.sentences);

            foreach (var sentence in pa.sentences)
            {
                pa.tokens = tokenizer.Tokenize(sentence);
                pa.partsOfSpeech = posTagger.Tag(pa.tokens);
            }
            return pa;
        }

    }
    */
}
